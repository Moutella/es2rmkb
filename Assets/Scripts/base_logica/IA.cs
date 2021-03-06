using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IA : MaoUsuario
{
	public MCTSNo monteCarloTree;
	public bool jogadorAtual;
	public ControladorJogo controlador;

	public IA(){
		this.pecas = new ArrayList();
        this.primeiraJogada = true;
        this.comprouPeca = false;
		controlador = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControladorJogo>();
	}
	public IA(ArrayList pecas,Boolean jogadorAtual){
		this.pecas=pecas;
		this.primeiraJogada=true;
		this.comprouPeca=false;
		this.jogadorAtual=jogadorAtual;
		controlador = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControladorJogo>();

	}
	public IA(MaoUsuario MaoUsuario){
		this.pecas=MaoUsuario.getPecas();
		this.primeiraJogada=MaoUsuario.getPrimeiraJogada();
		this.comprouPeca= MaoUsuario.getComprouPeca();
		this.controlador= GameObject.FindGameObjectWithTag("GameController").GetComponent<ControladorJogo>();
	}

	public void conjuntosBacktracking(ArrayList grupos, ArrayList respAtual, ArrayList resp, int k){
		for(int i=k;i<grupos.Count;i++){
			if(this.saoDisjuntos((Conjunto)grupos[i],respAtual)){
				respAtual.Add(grupos[i]);
				resp.Add((ArrayList)respAtual.Clone());
				conjuntosBacktracking(grupos, respAtual, resp, i);
				respAtual.Remove(grupos[i]);
			}	
		}
	}

	public ArrayList retornaJogadasPossiveis(){

		

		if(!this.getPrimeiraJogada()){
			//ArrayList insercoes = retornaInsercoes(controlador.getTabuleiroAtual());
			//ArrayList removendoDeGrupos = retornaRemoveDeGrupo(controlador.getTabuleiroAtual());
			ArrayList tudo = retornaTUDO(controlador.getTabuleiroAtual());
			//jogadas.AddRange(insercoes);
			return tudo;
		}else{
			ArrayList conjuntosDaMao = retornaTodosOsConjuntosDaMao(this.pecas);
			ArrayList jogadasDaMao = transformaTodosOsArrayListsEmJogadas(conjuntosDaMao);

			ArrayList jogadas = jogadasDaMao;
			for(int i=jogadas.Count-1;i>=0;i--){
				if(((Jogada)jogadas[i]).contaPontos()<30){
					jogadas.Remove(jogadas[i]);
				}
			}

			return jogadas;
		}
		



		
	}


	public ArrayList retornaTUDO(Tabuleiro tabuleiro){
		ArrayList mao = (ArrayList)this.pecas.Clone();
		ArrayList pecasTabuleiro = new ArrayList();
		foreach(Conjunto c in tabuleiro.getConjuntos()){
			foreach(Peca p in c.getPecas()){
				mao.Add(p);
				pecasTabuleiro.Add(p);
			}
		}

		//printaPecas(mao);
		//printaPecas(pecasTabuleiro);

		ArrayList conjuntos = retornaTodosOsConjuntosDaMao(mao);
		
		//printaConjuntos(conjuntos);
		for(int i=conjuntos.Count-1;i>=0;i--){
			if(!jogadaValida((ArrayList)conjuntos[i], pecasTabuleiro, mao)){
				conjuntos.Remove(conjuntos[i]);
			}
		}

		ArrayList jogadas = transformaTodosOsArrayListsEmJogadas(conjuntos);

		return jogadas;


	}

	public void printaPecas(ArrayList al){
		Debug.Log("Vai começar");
		foreach(Peca p in al){
			Debug.Log("Peca: "+p.getCodigoCor()+" "+p.getValor());
		}
	}

	public void printaConjuntos(ArrayList al){
		Debug.Log("Vai começar");
		foreach(ArrayList array in al){
			Debug.Log("Jogada");
			foreach(Conjunto c in array){
				c.printaPecas();
				Debug.Log("Prox");
			}
			
		}
	}

	public bool jogadaValida(ArrayList al, ArrayList pecasTabuleiro, ArrayList mao){
		ArrayList usadas = new ArrayList();
		foreach(Conjunto c in al){
			foreach(Peca p in c.getPecas()){
				usadas.Add(p);
			}
		}

		foreach(Peca p in pecasTabuleiro){
			if(!usadas.Contains(p)){
				return false;
			}
		}

		if(this.pecas.Count==mao.Count-usadas.Count) return false;
		else return true;
	}




	public ArrayList retornaTodosOsConjuntosDaMao(ArrayList mao){
		ArrayList grupos = retornaTodosOsGrupos(mao);
		ArrayList sequencias = retornaTodasAsSequencias(mao);

		sequencias.AddRange(grupos);


		ArrayList jogadas = new ArrayList();
		ArrayList jogadaAtual = new ArrayList();
		
		conjuntosBacktracking(sequencias, jogadaAtual, jogadas, 0);

		return jogadas;
		
	}

	public ArrayList retornaTodasAsSequencias(ArrayList mao){
		Conjunto conjunto = new Conjunto();
		ArrayList resp = new ArrayList();
		int pecasNoConjunto = 0;
		int ultimaCor = -1;
		IComparer comparador = Peca.getComparadorPorCores();
        mao.Sort(comparador);

		int i, j;
		for(i = 0; i < mao.Count; i++) {
			Peca pivo = (Peca) mao[i];
			conjunto.inserePeca(pivo);
			ultimaCor = pivo.getCodigoCor();
			pecasNoConjunto = 1;

			for(j = i+1; j < mao.Count; j++) {
				Peca auxiliar = (Peca) mao[j];
				if(ultimaCor != auxiliar.getCodigoCor()) break; //Caso onde eu já to olhando uma cor diferente da primeira peça da sequencia
				
				conjunto.inserePeca(auxiliar); //Insiro a possivel proxima peça valida
				pecasNoConjunto += 1; //Atribuo 1 ao número de elementos no conjunto
				if(conjunto.getValida()) resp.Add(conjunto.cloneConjunto()); // Se o conjunto estiver válido com esse elemento adicionado, adiciono na resposta
				else if (pecasNoConjunto >= 3) { // Caso onde o conjunto já ficou inválido, e já tem mais de 2 peças no conjunto, ou seja ele nao ficar mais valido se eu adicionar mais peças
					conjunto.limpaConjunto();
					break;
				}
			}
		}

		return resp;
	}

	public ArrayList retornaTodosOsGrupos(ArrayList mao){
		IComparer comparador = Peca.getComparadorSequencial();
        pecas.Sort(comparador);
		Peca coringa = null;
		Peca anterior = null;
		int n=0;
		Conjunto conjunto = new Conjunto();
		ArrayList resp = new ArrayList();
		foreach(Peca p in mao){
			if(!p.ehCoringa()){
				if(anterior==null){
					conjunto.inserePeca(p);

				}else{
					if(anterior.getValor()==p.getValor()){
						if(anterior.getCodigoCor()!=p.getCodigoCor()){
							//Nunca vai acontecer da cor ser diferente da peça imediatamente anterior mas igual a outra peça do conjunto
							//porque a ordenação sequencial também ordena secundariamente por cor
							conjunto.inserePeca(p);
						}
					}else{
						//Se são de valores diferentes este conjunto já não tem mais como aumentar(a não ser por coringas)
						n = conjunto.getNumPecas();
						if(n<4){
							coringa = achaCoringaForaDoConj(conjunto, mao);
							while(coringa!=null && conjunto.getNumPecas()<4){
								//NO CASO DO GRUPO NÃO HÁ ORDEM, ENTÃO A LINHA ABAIXO DEVE FUNCIONAR CORRETAMENTE
								conjunto.inserePeca(coringa);
								if(conjunto.getValida()) resp.Add(conjunto.cloneConjunto());
								coringa = achaCoringaForaDoConj(conjunto, mao);
							}
						}

						//Tomar cuidado, pois o conjunto continuaria "válido" se eu não iniciasse um novo com a peça atual
						conjunto.limpaConjunto();
						conjunto.inserePeca(p);

							
					}
				}
				//Verifica se o conjunto atual é válido. Se for, add no array de resposta
				if(conjunto.getValida()) resp.Add(conjunto.cloneConjunto());		
			}

			anterior = p;
		}

		addPermutacoes(resp);

		return resp;
	}

	public void addPermutacoes(ArrayList conjuntos){
		ArrayList add = new ArrayList();
		Conjunto novo;
		for(int k=conjuntos.Count-1;k>=0;k--){
			if(((Conjunto)conjuntos[k]).tipo==0 && ((Conjunto)conjuntos[k]).getNumPecas()==4){
				for(int i=0;i<3;i++){
					novo = new Conjunto();
					for(int j=0;j<4;j++){
						if(i!=j){
							novo.inserePeca((Peca)((Conjunto)conjuntos[k]).getPecas()[j]);
						}
					}
					conjuntos.Add(novo);
				}
			}
		}
	}


	public ArrayList retornaInsercoes(Tabuleiro tabuleiro){
		//Tabuleiro tabuleiro = controlador.getTabuleiroAtual();
		Conjunto clone = null;
		ArrayList jogadas = new ArrayList();

		Jogada jogadaAtual = new Jogada();
		foreach(Conjunto c in tabuleiro.getConjuntos()){
			clone = c.cloneConjunto();
			retornaInsercoesBacktrackingFim(clone, jogadaAtual,jogadas, 0, c);
			//Fazer o do inicio
			if(c.tipo!=0){
				retornaInsercoesBacktrackingInicio(clone, jogadaAtual,jogadas, this.pecas.Count-1, c);
			}
			
		}

		return jogadas;
	}

	public void retornaInsercoesBacktrackingFim(Conjunto clone, Jogada jogadaAtual, ArrayList jogadas, int i, Conjunto real){
		//Debug.Log(i +", "+this.pecas.Count);
		if(i>=this.pecas.Count)return;
		
		Peca p = (Peca)this.pecas[i];
		clone.inserePeca(p);
		if(clone.getValida()){
			jogadaAtual.insereSubJogada(new SubJogada(p, SubJogada.INS, real, false));
			jogadas.Add(jogadaAtual.clonaJogada());
			retornaInsercoesBacktrackingFim(clone, jogadaAtual, jogadas, i+1, real);
			jogadaAtual.subjogadas.Clear();
		}
		clone.removePeca(p);
		retornaInsercoesBacktrackingFim(clone, jogadaAtual, jogadas, i+1, real);
	}

	public void retornaInsercoesBacktrackingInicio(Conjunto clone, Jogada jogadaAtual, ArrayList jogadas, int i, Conjunto real){
		if(i<0)return;

		Peca p = (Peca)this.pecas[i];
		clone.inserePecaAntes(p);
		if(clone.getValida()){
			jogadaAtual.insereSubJogada(new SubJogada(p, SubJogada.INS, real, true));
			jogadas.Add(jogadaAtual.clonaJogada());
			retornaInsercoesBacktrackingInicio(clone, jogadaAtual, jogadas, i-1, real);
			jogadaAtual.subjogadas.Clear();
		}
		clone.removePeca(p);
		retornaInsercoesBacktrackingInicio(clone,jogadaAtual,jogadas,i-1, real);
	}


	public Peca achaCoringaForaDoConj(Conjunto c, ArrayList mao){
		foreach(Peca p in mao){
			if(p.ehCoringa() && !c.getPecas().Contains(p)){
				return p;
			}
		}

		return null;
	}

	public bool saoDisjuntos(Conjunto c, ArrayList al){
		foreach(Conjunto c2 in al){
			if(!c.ehDisjunto(c2)) return false;
		}

		return true;
	}



	public ArrayList transformaTodosOsArrayListsEmJogadas(ArrayList al){
		ArrayList resp = new ArrayList();
		foreach(ArrayList array in al){
			resp.Add(transformaArrayListEmJogada(array));
		}

		return resp;
	}


	public Jogada transformaArrayListEmJogada(ArrayList al){
		Jogada jogada = new Jogada();

		foreach(Conjunto c in al){
			jogada.insereSubJogada(new SubJogada(null, SubJogada.NOVO, c));
		}

		return jogada;
	}
	public IA clone(){
        IA newUsuario=(IA)new MaoUsuario();//Pq não new IA()?
        newUsuario.primeiraJogada=this.primeiraJogada;
        newUsuario.comprouPeca=this.comprouPeca;
        newUsuario.pecas=(ArrayList)this.pecas.Clone();
		newUsuario.controlador=this.controlador;
		newUsuario.jogadorAtual=this.jogadorAtual;
        return newUsuario;
    }

	public void jogar(Jogada jogada,Tabuleiro tabuleiro){
		foreach(SubJogada sj in jogada.subjogadas){
			int tipo=sj.tipo;
			if (tipo==0){   //Split

			}else if(tipo==1){      //inserção

			}else if(tipo==2){      //Novo
				tabuleiro.insereConjunto(sj.pai);
				foreach(Peca peca in sj.pai.getPecas()){
					this.removePeca(peca);	
				}		
			}else if(tipo==3){      //Move

			}
		}
		//return true;
	}
	
	public Jogada monteCarlo(MaoUsuario jogador,Tabuleiro tabuleiro){
			//Monte carlo possui 4 etapas:Seleção, expansão, simulação e backpropagation
			//Seleção: Escolhe uma jogada do estado atual(não é 100% aleatorio, ele leva em consideraçào as jogadas anteriores)
			//Expansão: Gera todas as jogadas possiveis do estado escolhido
			//Simulação: Escolhe aleatoriamente uma jogada possivel, passando pro nó novo e chamando novamente a expansão
			//Backpropagation: Volta o resultado(vitoria/derrota/empate) para o nó acima até chegar na raiz
			// Pra fazer o monte carlo é preciso criar uma estrutura que vai conter: O pai da jogada atual, quantidade de vitorias, as possiveis jogadas geradas por ele, e a quantidade de vezes que este nó foi visitado 
		return null;
	}

	public void jogarAleatorio(ArrayList jogadas,Tabuleiro tabuleiro){
			int tamArray=jogadas.Count;
			System.Random rnd=new System.Random();
			Jogada escolhido=(Jogada)jogadas[rnd.Next(tamArray)];
			jogar(escolhido,tabuleiro);	
	}

	public Jogada retornaJogadaAleatoria(){
		ArrayList jogadas = retornaJogadasPossiveis();
		int tamArray=jogadas.Count;
		Jogada escolhido;
		if(tamArray>0){
			System.Random rnd=new System.Random();
			escolhido=(Jogada)jogadas[rnd.Next(tamArray)];
		}else{
			escolhido=null;
		}
		
		return escolhido;
	}

	public Jogada retornaJogadaOrdenada(){
		ArrayList jogadas = retornaJogadasPossiveis();

		int tamArray=jogadas.Count;
		Jogada escolhido;
		if(tamArray>0){
			jogadas.Sort();
			escolhido=(Jogada)jogadas[0];
		}else{
			escolhido=null;
		}
		
		return escolhido;
	}

}