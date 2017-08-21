
import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;

import javax.websocket.OnClose;
import javax.websocket.OnError;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;


@ServerEndpoint(value = "/stringdemo")
public class SampleWebSocket {
	private Session _session;
	private Timer timer;
	private static int _count = 1;
	private static String _text;

	@OnOpen
    public void open(Session session) { 
        //Connection opened.
        System.out.println("EchoEndpoint on open");         
    }
	
	@OnMessage
    public void onMessage(String message, Session session) {
        _session = session;
        _text = message;
        StartTimer();
    }
	
	@OnError
    public void error(Session session, Throwable error) { 
        //Connection error.
        System.out.println("EchoEndpoint on error");
    }

    @OnClose
    public void close(Session session) { 
        //Connection closed.
        System.out.println("EchoEndpoint on close");
        StopTimer();
    }
    
    private void StartTimer()
    {
    	timer = new Timer();
        timer.schedule(new TimerTask() {

            @Override
            public void run() {
            	String message = _text + Integer.toString(_count);
            	_count++;
            	try {
            		synchronized(this){
            			_session.getBasicRemote().sendText(message);
            		    }					
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
            }
        }, 0, 10000);
    }
    
    private void StopTimer()
    {
    	timer.cancel();    
    }
	
}
