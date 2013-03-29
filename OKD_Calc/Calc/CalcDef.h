/////////////////////////////////////////////////////////////////////
//
//  CalcDef.h
//  クラス
// 
//	2009-10-08	作成	中岡昌俊


#ifndef CALCDEF_H
#define CALCDEF_H


//デバッグ用の表示
//#define	 DEBUG_PRINT1		0
//#define	 DEBUG_PRINT2		1			//debug
//#define	 DEBUG_PRINT3		1			//debug
//#define	 DEBUG_PRINT4		0			//debug
//#define	 DEBUG_PRINT5		0			//debug


#define MAXSTRRPN	2048

enum returnCode{ rErrOK = 0, rErrNG = 1 };


enum errRun { rErrSystem = 1 } ;
enum errIniFile { iErrOK = 0, iErrNoFile = 1000, iErrRead, iErrMeas, iErrTest, iErrExp } ;
enum errMeasSet { mErrOK = 0, mErrNoCh = 2000, mErrCh } ;
enum errTestSet { tErrOK = 0, tErrNoCh = 3000, tErrCh } ;
enum errExpression{ eErrOK = 0, eErrNoCh = 4000, eErrLvalue,
					eErrAssinmentOperator, eErrNoExp, eErrParenthesis, eErrFunction,
                    eErrExtraParameter, eErrTooFewParameter,
                    eErrRparenthesis, eErrLparenthesis,
                    eErrValue, eErrOVFvalue, eErrUndefined,
                    eErrSyntax } ;
enum errWarning { wErrForwardRef = 5000 } ;

//char    *_errMsg( int e ) ;



#endif
