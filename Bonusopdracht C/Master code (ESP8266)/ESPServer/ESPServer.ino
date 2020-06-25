#include <HTTPSRedirect.h>
#include <ESP8266WiFi.h>

const char* ssid     = "Modem";
const char* password = "Mijn wachtwoord";

const char *GScriptId = "AKfycbxrb3ZCGKvLJLzyEPDBZVqEgH8xYl7TU77BJ2zSteHzfRSUUTcs";

const char* googleHost      = "script.google.com";
const int httpsPort         =  443;

HTTPSRedirect* httpsClient = nullptr;

int MAXHTTPTRY = 5;

// Prepare the url (without the varying data)
String url = String("/macros/s/") + GScriptId + "/exec?";

//const char* fingerprint = "D7 C0 89 A6 79 98 FD A5 B1 42 30 BA 44 E0 4A 15 D7 1C BD EB";

// WifiServer creation on port 301
WiFiServer server(80);

// Variable to store the HTTP request
String header;

String nameSender = "";
String valueSensor = "";

int loc1 = 0;
int loc2 = 0;
int loc3 = 0;

bool startHttpsClient(uint8_t pin = 0, bool print = false) {
  httpsClient = new HTTPSRedirect(httpsPort);
  httpsClient->setInsecure();
  httpsClient->setPrintResponseBody(print);
  httpsClient->setContentTypeHeader("application/json");
  bool httpConnected = false;
  Serial.print("Connecting to https client ...");
  uint8_t i = 0;
  while ((httpsClient->connect(googleHost, httpsPort) != 1) && (i < MAXHTTPTRY)) {
    Serial.print(" "); Serial.print(i);
    if (pin > 0) Serial.println("Attempting to connect"); else delay(100);
    i++;
  }
  Serial.println();
  httpConnected = (i < MAXHTTPTRY);
  //(client.verify(fingerprint, host)) ? Serial.println("Certificate match") : Serial.println("Certificate mis-match");
  if (httpConnected) Serial.print("Connected to: "); Serial.print(googleHost); Serial.print(" ("); Serial.print(httpsPort); Serial.println(")");
  return httpConnected;
}

void getHttpsClient(String url, String data) {
  Serial.print("Get: "); Serial.println(String(googleHost) + url + data);
  httpsClient->GET(url + data, googleHost, false);
}

void writeHttpsClient(String url, String data) {
  getHttpsClient(url, data);
}

void postHttpsClient(String url, String data, String sheet = "Groep14") {
  String payload = "{\"command\": \"appendRow\", \"sheet\": \"" + sheet + "\", \"values\": \"" + data + "\"}";
  Serial.print("Post: "); Serial.println(String(googleHost) + url + payload);
  httpsClient->POST(url, googleHost, payload, false);
}

// write to tag, value to default sheet
void writeGoogleData(String url, String tag, String value, String sheet) {
  String arg = "tag=" + tag + "&value=" + value + "&sheet=" + sheet;
  if (httpsClient->connected())getHttpsClient(url, arg);
}

void setup() {
  Serial.begin(115200);
  // Initialize the output variables as outputs

  // Connect to Wi-Fi network with SSID and password
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  server.begin();
}



void loop() {
  WiFiClient client = server.available();   // Listen for incoming clients
  delay(250);
  if (client) {                             // If a new client connects,
    Serial.println("New Client.");          // print a message out in the serial port
    String currentLine = "";                // make a String to hold incoming data from the client
    while (client.connected()) { // loop while the client's connected
      if (client.available()) {             // if there's bytes to read from the client,
        char c = client.read();             // read a byte, then
        Serial.write(c);                    // print it out the serial monitor
        header += c;
        if (c == '\n') {                    // if the byte is a newline character
          // if the current line is blank, you got two newline characters in a row.
          // that's the end of the client HTTP request, so send a response:
          if (currentLine.length() == 0) {

            if (header.indexOf("GET /v") >= 0) {
              //Serial.println("De GET is ontvangen");
              loc1 = header.indexOf('v') + 1;
              loc2 = header.indexOf('&') + 1;
              loc3 = header.indexOf('-');
              nameSender = header.substring(loc1, loc2 - 1);
              valueSensor = header.substring(loc2, loc3);
              valueSensor.replace(".", ",");
              String nameFile = "Groep14";
              Serial.print("De waardes van nameSender en valueSensor zijn respectievelijk: ");
              Serial.println(nameSender + " " + valueSensor);
              if (startHttpsClient()) {
                writeGoogleData(url, nameSender, valueSensor, nameFile);
                delay(3000);
                Serial.println("Sluiten van https client");
                httpsClient->stop();
                delete httpsClient;
                loc1 = loc2 = loc3 = 0;
                nameSender = valueSensor = "";
              }
              else {
                Serial.println("Geen verbinding met google");
              }
            }

            // Prepare the response
            String s = "HTTP/1.1 200 OK\r\n";
            s += "Content-Type: text/html\r\n\r\n";
            s += "<!DOCTYPE HTML>\r\n<html>\r\n";
            s += "<head><title>Iwan opdracht C webserver</title><link rel=\"icon\" href=\"data:,\"></head>";
            s += "<style> body{font-family:verdana;background-color:LightBlue;} </style>";
            s += "<body>";
            s += "<br><h1 style=\"text-align:center\"> Welkom op de invulpagina </h1>";
            s += "<br><br><br>";
            s += "<br><h2 style=\"text-align:center\"> Om iets te versturen moet je GET variabelen gebruiken. Syntax is als volgt: </h2>";
            s += "<br>";
            s += "<br><h2 style=\"text-align:center\"> Webadres:Poort/vNaamVerstuurder&WaardeVariabele- <br> </h2>";
            s += "<br><h2 style=\"text-align:center\"> Bijvoorbeeld: http://77.167.204.165:80/vIwanArduino&28- </h2>";
            s += "</body>";
            s += "</html>\n";

            client.flush();


            // Send the response to the client
            client.print(s);
            // Break out of the while loop
            break;
          } else { // if you got a newline, then clear currentLine
            currentLine = "";
          }
        } else if (c != '\r') {  // if you got anything else but a carriage return character,
          currentLine += c;      // add it to the end of the currentLine
        }
      }
    }
    // Clear the header variable
    header = "";
    // Close the connection
    client.stop();
    Serial.println("Client disconnected.");
    Serial.println("");
  }
}
