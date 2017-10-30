
import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;

import javax.websocket.OnClose;
import javax.websocket.OnError;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;

import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;


@ServerEndpoint(value = "/jsondemo")
public class JsonWebsocket {
	private Session _session;
	private Timer timer;
	private String _message;

	@OnOpen
    public void open(Session session) { 
        //Connection opened.
        System.out.println("EchoEndpoint on open");
        _session = session;
    }
	
	@OnMessage
    public void onMessage(String message, Session session) {
		_session = session;
		_message= message;
		StartTimer();
    }
	
	private Result CalculateResult(Data data) {
		int addition = data.getNumber1() + data.getNumber2();
		Result result = new Result();
		result.setValue(addition);
		result.setId(data.getId());
		return result;
	}
	
	private void StartTimer()
    {
    	timer = new Timer();
        timer.schedule(new TimerTask() {

            @Override
            public void run() {
            	String message = getResultMessage();
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
	
	private String getResultMessage()
	{
		String resultMessage = "";
		ObjectMapper mapper = new ObjectMapper();
		mapper.configure(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES, true);
		try {
			Data data = mapper.readValue(_message, Data.class);
			Result result = CalculateResult(data);
			resultMessage = mapper.writeValueAsString(result);
			
		} catch (JsonParseException e) {
			e.printStackTrace();
		} catch (JsonMappingException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
		return resultMessage;
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
    
    
    private void StopTimer()
    {
    	timer.cancel();    
    }
	
}
