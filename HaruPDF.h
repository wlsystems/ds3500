
/*Dustin Shiozaki cs3505 A2 */

#include "hpdf.h"
#ifndef HARUPDF_H
#define HARUPDF_H
class HaruPDF{
   HPDF_Doc pdf;
   HPDF_Page page;
   char fname[256];
   HPDF_Font font;
   const char* SAMP_TXT;
public:
   HaruPDF(int argc, char **argv);
   
};

#endif