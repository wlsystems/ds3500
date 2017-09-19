/*
 * << Alternative PDF Library 1.0.0 >> -- text_demo2.c
 *
 * Copyright (c) 1999-2006 Takeshi Kanno <takeshi_kanno@est.hi-ho.ne.jp>
 *
 * Permission to use, copy, modify, distribute and sell this software
 * and its documentation for any purpose is hereby granted without fee,
 * provided that the above copyright notice appear in all copies and
 * that both that copyright notice and this permission notice appear
 * in supporting documentation.
 * It is provided "as is" without express or implied warranty.
 *
 * Modified by David Johnson, University of Utah, 2016.
 //Dustin Shiozaki cs3505 A2
 */

using namespace std;
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <iostream>
#include "HaruPDF.h"
#include "Spiral.cpp"
#include "Point.cpp"



// argc is the number of arguments. Argv is an array of character arrays, or C-style strings.
// If you call the program like ./pdfExample "Hello", then argv[1] would contain "Hello\0".
// argv[0] would be "pdfExample\0" - the name of the executing program.

HaruPDF::HaruPDF(int argc, char **argv) {
	pdf = HPDF_New (NULL, NULL);
	page = HPDF_AddPage (pdf);
	strcpy (fname, argv[0]);
    strcat (fname, ".pdf");
    font = HPDF_GetFont (pdf, "Helvetica", NULL);

    // argv are the command line arguments
    // argv[0] is the name of the executable program
    // This makes an output pdf named after the program's name
    strcpy (fname, argv[0]);
    strcat (fname, ".pdf");
    HPDF_Page_SetSize (page, HPDF_PAGE_SIZE_A5, HPDF_PAGE_PORTRAIT);
//    print_grid  (pdf, page);
    HPDF_Page_SetTextLeading (page, 20);
    HPDF_Page_SetGrayStroke (page, 0);
    SAMP_TXT =  argv[1];
    //SAMP_TXT = "The quick brown fox jumps over the lazy dog. We need more text to test a spiral. Maybe the radians needs to increase with smaller radius. ";
    HPDF_Page_BeginText (page);
    HPDF_Page_SetFontAndSize (page, font, 30);
    unsigned int i;
    Spiral p(0.0, 0.0, 400, 0.0);


	for (i = 0; i < strlen (SAMP_TXT); i++) {

        char buf[2];

        // This ugly function defines where any following text will be placed
    	// on the page. The cos/sin stuff is actually defining a 2D rotation
    	// matrix.
    	HPDF_Page_SetTextMatrix(page, cos(p.getRad1()), sin(p.getRad1()), 
    	-sin(p.getRad1()), cos(p.getRad1()), p.getTextX(), p.getTextY());

        // C-style strings are null-terminated. The last character must a 0.
        buf[0] = SAMP_TXT[i]; // The character to display

        buf[1] = 0;
        HPDF_Page_ShowText (page, buf);
        p++;

	}


    HPDF_Page_EndText (page);

    /* save the document to a file */
    HPDF_SaveToFile (pdf, fname);

    /* clean up */
    HPDF_Free (pdf);
}







