// ClusterCplusplus.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <math.h>
#include <fstream>

using namespace std;

#pragma region Class
struct parametrs
{
public:
	double ETA;
	double kolvoatom;
	//double radiuscluster;
	double radiusatomov;
	double kolvopovtorenii;
};
class cluster
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
class atom
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

int _tmain(int argc, _TCHAR* argv[])
{
	
	return 0;
}

