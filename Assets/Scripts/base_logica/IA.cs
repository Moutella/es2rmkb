using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA
{

	private bool primeiraJogada;
	private MaoUsuario mao;

	public IA(MaoUsuario m)
	{
		primeiraJogada = true;
		this.mao = m;
	}

	public bool getPrimeiraJogada() { return this.primeiraJogada; }
	public void setPrimeiraJogada(bool pj) { this.primeiraJogada = pj; }

	public MaoUsuario getMao() { return this.mao; }
	public void setMao(MaoUsuario m) { this.mao = m; }

	public void jogada()
	{
		if (primeiraJogada)
		{
			/*
			procura Conjunto >= 30 pontos
			if (Conjunto existe)
			{
				realiza jogada
				this.primeiraJogada = false;
			}
			else
			{
				saca do Deck
			}
			*/
		}
		else
		{
			/*
			procura jogada possivel
			if (jogada existe)
			{
				realiza jogada
			}
			else
			{
				saca do Deck
			}
			*/
		}
	}

}