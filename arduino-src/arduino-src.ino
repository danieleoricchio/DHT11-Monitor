#include "DHT.h"
#include <LiquidCrystal.h>

#define DHTPIN 8
#define DHTTYPE DHT11

DHT dht(DHTPIN, DHTTYPE);
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

void setup() {
  Serial.begin(9600);
  lcd.begin(16,2);
  dht.begin();
}

void loop() {

  float h = dht.readHumidity();

  float t = dht.readTemperature();

  if (isnan(h) || isnan(t)) {
    Serial.println("Impossibile leggere dal sensore dht");
    return;
  }

  lcd.setCursor(0,0);
  lcd.print("Temp: " + (String)t + char(223) + "C");

  lcd.setCursor(0,1);
  lcd.print("Umidita: " + (String)h + " %");

  Serial.print("@");
  Serial.print(t); Serial.print("A");
  Serial.print(h); Serial.print("B");
  Serial.print("\n");

  delay(500);
}
