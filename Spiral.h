/* Example code for CS3505 lecture 3
 * shows building a simple Spiral class
 * David Johnson
 */
//#include <iostream>
/*Dustin Shiozaki cs3505 A2 */
using namespace std;
#ifndef SPIRAL_H
#define SPIRAL_H
// The Spiral class holds 2D Spirals and lets you scale them and output them
class Spiral {
  double centerX, centerY, startRadius, startAngle;
  double textX;
  double textY;
  double angle2;
  float rad1;
  float rad2;
  unsigned int i;
public: // accessible methods
  // Constructors
  //Spiral();
  Spiral(double centerX, double centerY, double startRadius, double startAngle); // initializer style
  void makeSpiral();
  double getTextX();
  double getTextY();
  float getRad1();
  double getSpiralAngle();
  double getTextAngle();
  void operator++ ();
  void operator++ (int);
  void recalc();
  // Let the overloaded << function use the private class members
  friend ostream& operator<<(ostream& output, Spiral s);
}; // end of the Spiral class definition

#endif
