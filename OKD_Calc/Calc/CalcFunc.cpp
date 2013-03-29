/////////////////////////////////////////////////////////////////////
//
//  CalcFunc.cpp
//  ƒNƒ‰ƒX
// 
//	2009-10-08	ì¬	’†‰ª¹r

#include "stdafx.h"

#include "CalcFunc.h"


CalcFunc::CalcFunc( void )
{
	string	str ;

    fMap.insert( make_pair( str = "EXP", 1 ) ) ;
    fMap.insert( make_pair( str = "ABS", 1 ) ) ;
    fMap.insert( make_pair( str = "SQRT", 1 ) ) ;
    fMap.insert( make_pair( str = "POWER", 2 ) ) ;
    fMap.insert( make_pair( str = "LN", 1 ) ) ;
    fMap.insert( make_pair( str = "LOG", 1 ) ) ;
    fMap.insert( make_pair( str = "LOG10", 1 ) ) ;
    fMap.insert( make_pair( str = "PI", 0 ) ) ;
    fMap.insert( make_pair( str = "IF", 3 ) ) ;
    fMap.insert( make_pair( str = "AND", 2 ) ) ;
    fMap.insert( make_pair( str = "OR", 2 ) ) ;
    fMap.insert( make_pair( str = "NOT", 1 ) ) ;
    fMap.insert( make_pair( str = "TRUE", 0 ) ) ;
    fMap.insert( make_pair( str = "FALSE", 0 ) ) ;

}
