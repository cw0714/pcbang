using System;
using System.Net; //네트워크 처리
using System.Net.Sockets; // 소켓 처리
using System.Threading; // 스레드 처리
using System.Text; // 문자열 처리

namespace pcbang
{
    public class Network
    {
        Form1 wnd = null; //채팅 창(Form1) 객체 변수
        Socket server = null; //서버 소켓(접속을 받는 소켓)으로 사용할 소켓
        Socket client = null; //서버일 경우 통신 소켓,
                              //클라이언트일 경우 접속 및 통신용 소켓
        Thread th = null; //스레드 처리
        public Network(Form1 wnd)
        { // 생성자
            this.wnd = wnd; //NetWork 클래스에서 Form1의 멤버 사용을 허용
        }
        // 채팅 서버 시작 : 클라이언트 접속을 받고 메시지를 수신
        public void ServerStart()
        {
            try
            { //서버 포트 번호를 7000번으로 지정
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 7000);
                server = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
                server.Bind(ipep); //소켓과 서버 ip, 포트 번호를 바인드
                server.Listen(10); //클라이언트 접속을 대기
                wnd.Add_MSG("채팅 서버 시작...");// 채팅 창(txt_info)에 메시지 추가
                client = server.Accept(); //클라이언트가 접속할 때 활성화
                                          // 접속한 클라이언트 ip 주소를 출력
                IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                wnd.Add_MSG(ip.Address + "접속...");
                //상대방의 메시지를 수신하는 Receive 메서드를 스레드로 생성
                th = new Thread(new ThreadStart(Receive));
                th.Start();
            }
            catch (Exception ex)
            { //채팅 서버에서 예외가 발생하면
                wnd.Add_MSG(ex.Message); //예외 메시지 txt_info에 출력
            }
        }
        // 채팅 서버 중지
        public void ServerStop()
        {
            try
            {
                if (client != null)
                {
                    if (client.Connected)
                    { //클라이언트가 접속된 상태라면
                        client.Close(); // 통신 소켓을 닫습니다.
                        if (th.IsAlive) // Receive 스레드가 실행중이라면
                            th.Abort(); // 스레드 종료
                    }
                }
                server.Close(); //서버 소켓을 닫습니다.
            }
            catch (Exception ex)
            { //예외 메시지 출력
                wnd.Add_MSG(ex.Message);
            }
        }
        // 채팅 서버와 연결 종료
        public void DisConnect()
        {
            try
            {
                if (client != null)
                { // 채팅 서버와 연결 되어있다면
                    if (client.Connected)
                        client.Close(); // 채팅서버와의 연결을 단절
                    if (th.IsAlive)
                        th.Abort(); //Receive 메서드 스레드를 중지
                }
                wnd.Add_MSG("채팅 서버 연결 종료!");
            }
            //채팅 서버 연결 해제와 스레드 종료시 예외가 발생하면
            catch (Exception ex)
            {
                wnd.Add_MSG(ex.Message); // 예외 메시지 출력
            }
        }
        // 상대방의 데이터 수신
        public void Receive()
        {
            try
            { //상대방과 연결되었다면
                while (client != null && client.Connected)
                {
                    // ReceiveData 메서드를 사용해 바이트 단위로 데이터를 읽기
                    byte[] data = ReceiveData();
                    wnd.Add_MSG("[상대방]" + Encoding.Default.GetString(data));
                }
            }
            catch (Exception ex) { wnd.Add_MSG(ex.Message); }
        }
        // 상대방에게 데이터 송신
        public void Send(string msg)
        {
            try
            {
                if (client.Connected)
                { // 상대방과 연결되어 있으면
                  // 문자열을 바이트 배열 형태로 변경합니다.
                    byte[] data = Encoding.Default.GetBytes(msg);
                    SendData(data); //바이트 배열을 상대방에 전송
                }
                else { wnd.Add_MSG("메시지 전송 실패!"); }
            }
            catch (Exception ex) { wnd.Add_MSG(ex.Message); }
        }
        private void SendData(byte[] data)
        {
            try
            {
                int offset = 0; //버퍼 내 위치
                int size = data.Length; //전송할 바이트 배열의 크기
                int left_data = size; //남은 데이터 량
                int send_data = 0; // 전송된 데이터 크기
                                   // 전송할 실제 데이터의 크기 전달
                byte[] data_size = new byte[4]; //정수형태로 데이터 크기 전송
                data_size = BitConverter.GetBytes(size);
                send_data = client.Send(data_size);
                // 실제 데이터 전송
                while (offset < size)
                {
                    send_data = client.Send(data, offset, left_data, SocketFlags.None);
                    offset += send_data;
                    left_data -= send_data;
                }
            }
            catch (Exception ex)
            {//전송중 예외가 발생하면 에러메시지 출력
                wnd.Add_MSG(ex.Message);
            }
        }
        private byte[] ReceiveData()
        {
            try
            {
                int offset = 0; int size = 0; //버퍼 내 위치, 수신할 데이터 크기
                int left_data = 0; int recv_data = 0;//남은 데이터, 수신한 데이터 크기
                                                     // 수신할 데이터 크기 알아내기
                byte[] data_size = new byte[4];
                recv_data = client.Receive(data_size, 0, 4, SocketFlags.None);
                size = BitConverter.ToInt32(data_size, 0); left_data = size;
                byte[] data = new byte[size]; //바이트 배열 생성
                                              // 서버에서 전송한 실제 데이터 수신
                while (offset < size)
                { //상대방이 전송한 데이터를 읽어옴
                    recv_data = client.Receive(data, offset, left_data, SocketFlags.None);
                    if (recv_data == 0) break;
                    offset += recv_data; left_data -= recv_data;
                }
                return data;
            }
            //데이터 수신중 예외가 발생하면 에러메시지 출력
            catch (Exception ex) { wnd.Add_MSG(ex.Message); return null; }
        }
    }
}