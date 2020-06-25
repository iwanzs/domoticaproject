/*
Dit bestand is om de meetwaarden van een DHT11 sensor te versturen naar de master
Met behulp van een clientconnectie naar de webserver op de master kan de naam
van de gebruiker en de temperatuur van de slave worden verstuurd.
*/

#include <dht.h>
#include <Ethernet.h>
#include <SPI.h>

dht DHT;

#define DHT11_PIN 7

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
char server[] = "77.167.204.165";

IPAddress ip(192, 168, 0, 177);
IPAddress myDns(192, 168, 0, 1);

EthernetClient client;

static String sender = "IwanArduino"; // Verander dit naar je eigen naam

unsigned long beginMicros, endMicros;
unsigned long byteCount = 0;
bool printWebData = true;  // set to false for better speed measurement

void setup() {
  Serial.begin(9600);

// Verbinding maken met internet (ethernetshield)
  if (Ethernet.begin(mac) == 0) {
    Serial.println("Failed to configure Ethernet using DHCP");
    // Check for Ethernet hardware present
    if (Ethernet.hardwareStatus() == EthernetNoHardware) {
      Serial.println("Ethernet shield was not found.  Sorry, can't run without hardware. :(");
      while (true) {
        delay(1); // do nothing, no point running without Ethernet hardware
      }
    }
    if (Ethernet.linkStatus() == LinkOFF) {
      Serial.println("Ethernet cable is not connected.");
    }
    // try to congifure using IP address instead of DHCP:
    Ethernet.begin(mac, ip, myDns);
  } else {
    Serial.print("DHCP assigned IP ");
    Serial.println(Ethernet.localIP());
  }
  // give the Ethernet shield a second to initialize:
  delay(1000);
  beginMicros = micros();
}


// Logt de waardes van de DHT11 sensor
String logData() {
  int chk = DHT.read11(DHT11_PIN);
  Serial.print("Temperatuur is: ");
  Serial.println(DHT.temperature);
  String temp = String(DHT.temperature);
  Serial.print("De luchtvochtigheid is: ");
  Serial.println(DHT.humidity);
  return temp;
}

// Deze method regelt het verbinden met de webserver en
// het versturen van de opgeslagen values
void sendData(String data, String sender) {
  Serial.print("connecting to ");
  Serial.print(server);
  Serial.println("...");

  // if you get a connection, report back via serial:
  if (client.connect(server, 80)) {
    Serial.print("connected to ");
    Serial.println(client.remoteIP());
    // Make a HTTP request:
    Serial.println("connected");

    client.print("GET /");
    client.print("v");
    client.print(sender);
    client.print("&");
    client.print(data);
    client.println("- HTTP/1.0");
    client.println("Host: 77.167.204.165");
    client.println("Connection: close");
    client.println();
  } else {
    // if you didn't get a connection to the server:
    Serial.println("connection failed");
  }
}

void loop() {
  sendData(logData(), sender);
  // Delay wanneer er weer wordt verstuurd
  delay(600000);

  
}
