
// argc is the number of arguments. Argv is an array of character arrays, or C-style strings.
// If you call the program like ./pdfExample "Hello", then argv[1] would contain "Hello\0".
// argv[0] would be "pdfExample\0" - the name of the executing program.
//Dustin Shiozaki cs3505 A2
#include "HaruPDF.cpp"
int main(int argc, char **argv)
{
	HaruPDF h(argc, argv);
    return 0;
}