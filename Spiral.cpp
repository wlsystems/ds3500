 
/*Dustin Shiozaki cs3505 A2 */
#include "Spiral.h"
#define _USE_MATH_DEFINES
 
#include <cmath>
//Spiral::Spiral(){;}

	Spiral::Spiral(double centerX, double centerY, double startRadius, double startAngle)  {
	centerX = centerX;
	centerY = centerY;
	angle2 = startAngle+90;
} 

double Spiral::getTextX(){
	return textX;
}

double Spiral::getTextY(){
	return textY;
}
double Spiral::getSpiralAngle(){
	return angle2;
}
float Spiral::getRad1(){
	return rad1;
}
double Spiral::getTextAngle(){
	return angle2-180;
}

void Spiral::makeSpiral(){
	
}


void Spiral::recalc(){

	rad1 = (angle2 - 90) / 180 * 3.14;
    rad2 = angle2 / 180 * 3.14;

    textX = 210 + cos(rad2)*startRadius;
    textY = 300 + sin(rad2)*startRadius;
	angle2 -= 10;
	startRadius += 1 ;
}

void Spiral::operator++() {
	recalc();
  };
void Spiral::operator++(int) {
    recalc();
  };
ostream& operator<<(ostream& output, Spiral s)
{
  output << "(" << s.getTextX() << "," << s.getTextY() << ")";
  return output;
}