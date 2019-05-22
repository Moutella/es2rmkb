﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maoUI : MonoBehaviour
{
    public MaoUsuario maoLogica;
    private RectTransform transformProprio;
    public GameObject pecaPrefab;
    public GameObject[] slots;
    private ArrayList pecasUsadasNaRodada;
    public ArrayList pecaUIObjects;
    // Start is called before the first frame update
    void Start()
    {
        transformProprio = GetComponent<RectTransform>();
        pecaUIObjects = new ArrayList();

        maoLogica = new MaoUsuario();
    }
    public GameObject getPrimeiroVazio()
    {

        foreach (GameObject slot in slots)
        {
            if (slot.GetComponent<slotMao>().vazio())
                {                
                    return slot;
            }
        }
        return null;
    }
    void Update()
    {
        
    }
    public void setMaoInicial(ArrayList mao)
    {
        maoLogica.insereMaoInicial(mao);
        arranjaPecas();
        
    }
    public void arranjaPecas()
    {
        foreach (Peca p in maoLogica.getPecas())
        {
            GameObject peca = Instantiate(pecaPrefab, this.transform);
            peca.GetComponent<pecaGameUI>().criaPeca(p);
            GameObject slot = getPrimeiroVazio();
            slot.GetComponent<slotMao>().preenche(peca);
            //Debug.Log("Preencheu: " + slot.name);
            peca.GetComponent<pecaDragUI>().slotAtual = slot;
            peca.GetComponent<RectTransform>().SetPositionAndRotation(slot.transform.position, Quaternion.identity);
            pecaUIObjects.Add(peca);
        }
    }
    public void liberaTodos()
    {
        foreach(GameObject p in pecaUIObjects)
        {
            Destroy(p);
        }
        foreach(GameObject slot in slots)
        {
            slot.GetComponent<slotMao>().libera();
        }
    }
    public void sortMao()
    {
        liberaTodos();
        maoLogica.arrumaPorCores();
        arranjaPecas();
    }
    public void comprarPeca()
    {
        
    }

    public void rollbackPecas() {
        maoLogica.rollbackPecas();
    }
    public int getPontosDaJogada(){
        return maoLogica.pontuacaoJogada();
    }
    public bool getPrimeiraJogada(){
        return maoLogica.getPrimeiraJogada();
    }
    public void setPrimeiraJogada(bool valor){
        maoLogica.setPrimeiraJogada(valor);
    }
    public void limpaJogada(){
        maoLogica.limpaJogada();
    }
    public bool jogouAlgumaPeca(){
        return maoLogica.jogouAlgumaPeca();
    }
    public bool estavaNaMao(Peca p){
        return maoLogica.estavaNaMao(p);
    }
    public void fazBackup() {
        maoLogica.saveBackupPeca();
    }
    public void removePeca(GameObject peca)
    {
        Peca p = peca.GetComponent<pecaGameUI>().getPeca();
        pecaUIObjects.Remove(peca);
        maoLogica.removePeca(p);
    }
    public void inserePeca(GameObject peca)
    {
        Peca p = peca.GetComponent<pecaGameUI>().getPeca();
        pecaUIObjects.Add(peca);
        maoLogica.inserePeca(p);
    }
}
