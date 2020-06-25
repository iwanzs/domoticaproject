// Supported KaKu devices -> find, download en install corresponding libraries
#define unitCodeApa3      60753714   // replace with your own code
#define unitCodeActionOld 15        // replace with your own code
#define unitCodeActionNew 60753714    // replace with your own code

// Include files.
#include <SPI.h>                  // Ethernet shield uses SPI-interface
#include <Ethernet.h>             // Ethernet library (use Ethernet2.h for new ethernet shield v2)
#include <NewRemoteTransmitter.h> // Remote Control, Gamma, APA3
#include <NewRemoteReceiver.h>
#include <RemoteTransmitter.h>    // Remote Control, Action, old model
#include <RCSwitch.h>           // Remote Control, Action, new model
#include <ArduinoJson.h> 
#include <dht.h>

dht DHT;

#define DHT11_PIN 7

// Set Ethernet Shield MAC address  (check yours)
byte mac[] = { 0x40, 0x6c, 0x8f, 0x36, 0x84, 0x8a }; // Ethernet adapter shield S. Oosterhaven
int ethPort = 3300;                                  // Take a free port (check your router)
char serverAdress[] = "api.openweathermap.org";    // name address for Google (using DNS)
int currentValue = 0;

EthernetClient client;

String apiKey= "bb369822394023a11017c9be8719d8cf"; 
char weatherMapAPI[] = "api.openweathermap.org"; 
String location= "slootdorp"; 

#define RFPin        3  // output, pin to control the RF-sender (and Click-On Click-Off-device)
#define lowPin       5  // output, always LOW
#define highPin      6  // output, always HIGH
#define switchPin    4  // input, connected to some kind of inputswitch
#define ledPin       8  // output, led used for "connect state": blinking = searching; continuously = connected
#define infoPin      9  // output, more information
#define analogPin    0  // sensor value

EthernetServer server(ethPort);              // EthernetServer instance (listening on port <ethPort>).

NewRemoteTransmitter apa3Transmitter(unitCodeApa3, RFPin, 256);  // APA3 (Gamma) remote, use pin <RFPin> 
bool isOn = NewRemoteCode::on;
bool isOff = NewRemoteCode::off;
//ActionTransmitter actionTransmitter(RFPin);  // Remote Control, Action, old model (Impulse), use pin <RFPin>
//RCSwitch mySwitch = RCSwitch();            // Remote Control, Action, new model (on-off), use pin <RFPin>

char actionDevice = 'A';                 // Variable to store Action Device id ('A', 'B', 'C')
bool pinState = false;                   // Variable to store actual pin state
bool pinChange = false;                  // Variable to store actual pin change
int  sensorValue = 0;                    // Variable to store actual sensor value
int temp = 0;

// Initialize the Ethernet client library
// with the IP address and port of the server
// that you want to connect to (port 80 is default for HTTP):


void setup()
{
   int chk = DHT.read11(DHT11_PIN);
   Serial.begin(9600);
   //while (!Serial) { ; }               // Wait for serial port to connect. Needed for Leonardo only.

//   Serial.println("Domotica project, Arduino Domotica Server\n");
   
   //Init I/O-pins
   pinMode(switchPin, INPUT);            // hardware switch, for changing pin state
   pinMode(lowPin, OUTPUT);
   pinMode(highPin, OUTPUT);
   pinMode(RFPin, OUTPUT);
   pinMode(ledPin, OUTPUT);
   pinMode(infoPin, OUTPUT);
   
   //Default states
   digitalWrite(switchPin, HIGH);        // Activate pullup resistors (needed for input pin)
   digitalWrite(lowPin, LOW);
   digitalWrite(highPin, HIGH);
   digitalWrite(RFPin, LOW);
   digitalWrite(ledPin, LOW);
   digitalWrite(infoPin, LOW);

   //Try to get an IP address from the DHCP server.
   if (Ethernet.begin(mac) == 0)
   {
//      Serial.println("Could not obtain IP-address from DHCP -> do nothing");
      while (true){     // no point in carrying on, so do nothing forevermore; check your router
      }
   }
   
//   Serial.print("LED (for connect-state and pin-state) on pin "); Serial.println(ledPin);
//   Serial.print("Input switch on pin "); Serial.println(switchPin);
//   Serial.println("Ethernetboard connected (pins 10, 11, 12, 13 and SPI)");
//   Serial.println("Connect to DHCP source in local network (blinking led -> waiting for connection)");
   
   //Start the ethernet server.
   server.begin();

   // Print IP-address and led indication of server state
//   Serial.print("Listening address: ");
   Serial.println(Ethernet.localIP());
   
   // for hardware debug: LED indication of server state: blinking = waiting for connection
   int IPnr = getIPComputerNumber(Ethernet.localIP());   // Get computernumber in local network 192.168.1.3 -> 3)
//   Serial.print(" ["); Serial.print(IPnr); Serial.print("] "); 
//   Serial.print("  [Testcase: telnet "); Serial.print(Ethernet.localIP()); Serial.print(" "); Serial.print(ethPort); Serial.println("]");
   signalNumber(ledPin, IPnr);
}

void loop()
{ 
   // Listen for incomming connection (app)
   EthernetClient ethernetClient = server.available();
   if (!ethernetClient) {
      blink(ledPin);
      return; // wait for connection and blink LED
   }


//   Serial.println("Application connected");
   digitalWrite(ledPin, LOW);

   // Do what needs to be done while the socket is connected.
   while (ethernetClient.connected()) 
   {      
      checkEvent(switchPin, pinState);          // update pin state
      delay(1);
//      sensorValue = readSensor(0, 100);         // update sensor value

      // Activate pin based op pinState
      if (pinChange) {
         if (pinState) { digitalWrite(ledPin, HIGH); switchDefault(true); }
         else { switchDefault(false); digitalWrite(ledPin, LOW); }
         pinChange = false;
      }
   
      // Execute when byte is received.
      while (ethernetClient.available())
      {
         char inByte = ethernetClient.read();   // Get byte from the client.
         executeCommand(inByte);                // Wait for command to execute
         inByte = NULL;                         // Reset the read byte.
      } 
   }
//   Serial.println("Application disonnected");
}

int getWeather() { 
// Serial.println("\nStarting connection to server...");  // if you get a connection, report back via serial: 
 if (client.connect(serverAdress, 80)) { 
   Serial.println("connected to server"); 
   // Make a HTTP request: 
   client.print("GET /data/2.5/weather?"); 
   client.print("q="+location); 
   client.print("&appid="+apiKey); 
   client.println("&units=metric"); 
   client.println("Host: api.openweathermap.org"); 
   client.println("Connection: close"); 
   client.println(); 
 } else { 
//   Serial.println("unable to connect"); 
 } 

 while (client.connected()) { 
   String line = client.readStringUntil('\n'); 
   //Serial.println(line); 
   
   //create a json buffer where to store the json data 
  StaticJsonDocument<475> doc;
  //Serial.println(doc.capacity());
 
   deserializeJson(doc, line); 

  int main_temp = doc["main"]["temp"]; // 25.95
  return main_temp;
 } 
} 

// Choose and switch your Kaku device, state is true/false (HIGH/LOW)
void switchDefault(bool state)
{   
  if(state == true){
   apa3Transmitter.sendUnit(15, isOn);          // APA3 Kaku (Gamma)                
   delay(100);
  }
  else{
    apa3Transmitter.sendUnit(15, isOff);     
    delay(100); 
  }
     delay(100);
  }
//   actionTransmitter.sendSignal(unitCodeActionOld, actionDevice, state);  // Action Kaku, old model
   //mySwitch.send(2210410 + state, 24);  // tricky, false = 0, true = 1  // Action Kaku, new model
   //delay(100);


// Implementation of (simple) protocol between app and Arduino
// Request (from app) is single char ('a', 's', 't', 'i' etc.)
// Response (to app) is 4 chars  (not all commands demand a response)
void executeCommand(char cmd)
{            
         char buf[4] = {'\0', '\0', '\0', '\0'};
         
         
         // Command protocol
         //Serial.print("["); Serial.print(cmd); Serial.print("] -> ");
         switch (cmd) {
         case 'a': // Report sensor value to the app  
            temp = DHT.temperature;
            Serial.println(temp);               
            sensorValue = temp * 1.8 + 32;       
            intToCharBuf(sensorValue, buf, 4);                // convert to charbuffer
            server.write(buf, 4);                             // response is always 4 chars (\n included)
            //Serial.print("Sensor: "); Serial.println(buf);
            break;
         case 's': // Report switch state to the app
            if (pinState) { server.write(" ON\n"); } //Serial.println("Pin state is ON"); }  // always send 4 chars
            else { server.write("OFF\n"); } //Serial.println("Pin state is OFF"); }
            break;
         case 't': // Toggle state; If state is already ON then turn it OFF
            if (pinState) { pinState = false; } //Serial.println("Set pin state to \"OFF\""); }
            else { pinState = true; } //Serial.println("Set pin state to \"ON\""); }  
            pinChange = true; 
            break;
         case 'i':    
            digitalWrite(infoPin, HIGH);
            break;
         case 'w':
            currentValue = getWeather();
            //Serial.println(currentValue);    
            intToCharBuf(currentValue, buf, 4);                // convert to charbuffer
            server.write(buf, 4);                             // response is always 4 chars (\n included)
            //Serial.print("Het weer: ");
            Serial.println(buf);
            currentValue = "";
            break;
         default:
            digitalWrite(infoPin, LOW);
         }
}

// read value from pin pn, return value is mapped between 0 and mx-1
int readSensor(int pn, int mx)
{
  return map(analogRead(pn), 0, 1023, 0, mx-1);    
}

// Convert int <val> char buffer with length <len>
void intToCharBuf(int val, char buf[], int len)
{
   String s;
   s = String(val);                        // convert tot string
   if (s.length() == 1) s = "0" + s;       // prefix redundant "0" 
   if (s.length() == 2) s = "0" + s;  
   s = s + "\n";                           // add newline
   s.toCharArray(buf, len);                // convert string to char-buffer
}

// Check switch level and determine if an event has happend
// event: low -> high or high -> low
void checkEvent(int p, bool &state)
{
   static bool swLevel = false;       // Variable to store the switch level (Low or High)
   static bool prevswLevel = false;   // Variable to store the previous switch level

   swLevel = digitalRead(p);
   if (swLevel)
      if (prevswLevel) delay(1);
      else {               
         prevswLevel = true;   // Low -> High transition
         state = true;
         pinChange = true;
      } 
   else // swLevel == Low
      if (!prevswLevel) delay(1);
      else {
         prevswLevel = false;  // High -> Low transition
         state = false;
         pinChange = true;
      }
}

// blink led on pin <pn>
void blink(int pn)
{
  digitalWrite(pn, HIGH); 
  delay(100); 
  digitalWrite(pn, LOW); 
  delay(100);
}

// Visual feedback on pin, based on IP number, used for debug only
// Blink ledpin for a short burst, then blink N times, where N is (related to) IP-number
void signalNumber(int pin, int n)
{
   int i;
   for (i = 0; i < 30; i++)
       { digitalWrite(pin, HIGH); delay(20); digitalWrite(pin, LOW); delay(20); }
   delay(1000);
   for (i = 0; i < n; i++)
       { digitalWrite(pin, HIGH); delay(300); digitalWrite(pin, LOW); delay(300); }
    delay(1000);
}

// Convert IPAddress tot String (e.g. "192.168.1.105")
String IPAddressToString(IPAddress address)
{
    return String(address[0]) + "." + 
           String(address[1]) + "." + 
           String(address[2]) + "." + 
           String(address[3]);
}

// Returns B-class network-id: 192.168.1.3 -> 1)
int getIPClassB(IPAddress address)
{
    return address[2];
}

// Returns computernumber in local network: 192.168.1.3 -> 3)
int getIPComputerNumber(IPAddress address)
{
    return address[3];
}

// Returns computernumber in local network: 192.168.1.105 -> 5)
int getIPComputerNumberOffset(IPAddress address, int offset)
{
    return getIPComputerNumber(address) - offset;
}
