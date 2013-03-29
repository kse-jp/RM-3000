/////////////////////////////////////////////////////////////////////
//
//  CalcMathErr.h
//  ƒNƒ‰ƒX
// 
//	2009-10-08	ì¬	’†‰ª¹r


#ifndef __CALCMATHERR_H
#define __CALCMATHERR_H

#include <string.h>
#include <stdio.h>

#pragma hdrstop

class	RunError
{
public:
	RunError( const RunError& e )	{ what = _strdup( e.what ) ; }
	RunError( char *s="Unknown" )	{ what = _strdup( s ) ; }
    ~RunError()                     { delete[] what ; }
    char	*msg() const			{ return what ; }
private:
	char	*what ;
} ;

extern char    *_errMsg( int e ) ;

#endif



