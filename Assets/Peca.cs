﻿using System;
using System.Collections;
public class Peca : IComparable
{
    private int codigoCor;
    private int valor;
    private bool coringa;

    public Peca(int cor, int valor, bool coringa){
        this.codigoCor=cor;
        this.valor=valor;
        this.coringa=coringa;
    }
    

    public int CodigoCor{
        get=> codigoCor;
        set=>codigoCor=value;
    }

    public int Valor{
        get=> valor;
        set=>valor=value;
    }

    public int CompareTo(Object obj){
        if(obj==null)return 1;

        Peca p = obj as Peca;
        if(p!=null){
            return this.valor.CompareTo(p.valor);
        }else
            throw new ArgumentException("Objeto comparado não é uma Peça");

    }

    public bool getCoringa(){return coringa;}
    public void setCoringa(bool value){coringa=value;}
}