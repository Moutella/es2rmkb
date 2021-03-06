﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private ArrayList pecas;
    private int quantasPecasTem = 106;
    
    public Deck()
    {
        pecas = new ArrayList();
        for(int i=0; i < 4; i++)
        {
            for(int j = 1; j < 14; j++)
            {
                for(int k = 0; k<2; k++)
                {
                    Peca pecaNova = new Peca(i, j, false);
                    this.pecas.Add(pecaNova);
                }
                
            }
        }
        pecas.Add(new Peca(0, -1, true));
        pecas.Add(new Peca(1, -1, true));
    }
    public Deck(ArrayList pecas,int quantasPecasTem){
        this.pecas=pecas;
        this.quantasPecasTem=quantasPecasTem;

    }
    public ArrayList pegaCartasIniciais()
    {
        ArrayList pecasCompradas = new ArrayList();
        for(int i = 0; i<14; i++)
        {
            Peca escolhida = (Peca)pecas[Random.Range(0, quantasPecasTem)];
            pecasCompradas.Add(escolhida);
            pecas.Remove(escolhida);
            quantasPecasTem--;
        }

        return pecasCompradas;
    }
    public Deck clone(){
        return new Deck((ArrayList)this.pecas.Clone(),this.quantasPecasTem);
    }
    public Peca pegaPecaAleatoria()
    {
        Peca p = (Peca)pecas[Random.Range(0, quantasPecasTem)];
        pecas.Remove(p);
        quantasPecasTem--;
        return p;
    }
    public void removeCoringas(){
        for(int i=0;i<2;i++){
            this.pecas.RemoveAt(this.pecas.Count-1);
        }
    }
}
