# include <iostream>
using namespace std;
void plotCharacter(int n ,char c){
	for (int i = 1; i <= n; i++){
		cout<<' ';
	}
	cout<<c;
}
void chartFunction(double r, double f, double sf){
	int rab = r;
	int fox = f;
	int total = r + f;
	if (rab == fox){
		plotCharacter(sf*r, '*');
		cout<<endl;
	}
	else if (rab < fox){
		plotCharacter(sf*rab, 'r');
		plotCharacter(sf*fox-sf*rab, 'F'); 
		cout<<endl;
	}
	else if (rab > fox){
		plotCharacter(sf*fox, 'F');
		plotCharacter(sf*rab-sf*fox, 'r'); 
		cout<<endl;
	}

}
void updatePopulations(double g, double p, double c, double m, double k,
                       double& numRabbits, double& numFoxes){
	chartFunction(numRabbits, numFoxes, .1);
	double dRab;
	double dFox;
	dRab = (g*numRabbits)*(1-numRabbits/k) - p*numRabbits*numFoxes;
	dFox = c*p*numRabbits*numFoxes - m*numFoxes;
    numRabbits += dRab;
    numFoxes += dFox;
}
void incrementCounter(int& i){
	i++;
}
int main(){
	double r;
	double f;
	cout <<"Enter the number of rabbits..." << endl;
	if (cin >> r){
		cout <<"Enter the number of foxes..." << endl;
		if (cin >> f){
			int i = 0;
			while(i < 500 && f>1 && r >1){
				updatePopulations(.2, .002, .6, .2, 1000, r, f);
				incrementCounter(i);
			}
		}
		else{
			cout<< "Invalid entry. Must be numerical value greater than 0." << endl;
		}
	}
	else {
		cout << "Invalid entry. Must be numerical value greater than 0." << endl;
	}

	return 0;
}