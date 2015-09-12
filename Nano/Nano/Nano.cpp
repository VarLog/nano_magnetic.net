// Nano.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <fstream>

using namespace std;
#pragma region structurs
struct parametrs
{
public:
	double ETA;
	double kolvoatom;
	//double radiuscluster;
	double radiusatomov;
	double kolvopovtorenii;
};
struct cluster
{
public:
	double R;
	double X;
	double Y;
	double Z;

	double Volume;

	double ETA;  // ETA:=NNp*Volume/VolCl;
	double Hdip; //Hdip:=Ms*ETA; 
	double Qint;

};
struct atom
{
public:
	double R;
	double X;
	double Y;
	double Z;
	double Volume;
	/*double M[1000];*/
};
#pragma endregion

atom randomAtom(double _R);

int main()
{
	double Pi = 3.16159265;
	ifstream fin("Parametrs.txt");
	double buf[50];
	parametrs Param[50];
	cluster LC[50];
	int i = 0;
	while (i != 50)
	{
		fin >> Param[i].ETA >> Param[i].kolvoatom >> Param[i].radiusatomov >> Param[i].kolvopovtorenii;
		LC[i].X = 0;
		LC[i].Y = 0;
		LC[i].Z = 0;
		LC[i].ETA = Param[i].ETA;
		LC[i].Volume = Param[i].kolvoatom * 4 * Pi*Param[i].radiusatomov / (3 * Param[i].ETA);
		LC[i].R = pow(LC[i].Volume, 1 / 3);
		atom LA[1000];
		for (int j = 0; j < 1000; j++)
		{
			LA[j] = randomAtom(Param[j].radiusatomov);
		}
		ofstream fout;
		fout.open("result.txt");
		fin.close();
		fout.close();
		i++;
	}
	return 0;
}

atom randomAtom(double _R)
{
	atom A;

	A.X = rand();
	A.Y = rand();
	A.Z = rand();
	A.R = _R;
	return A;
}


