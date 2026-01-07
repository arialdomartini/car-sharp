## Contesto

Stai sviluppando il core di un **servizio di car renting**.
Il sistema non ha interfaccia grafica: è una libreria che gestisce **stato**, **regole di business** e **decisioni automatiche**.
Il dominio viene introdotto **per passi**, partendo da un modello volutamente povero e arricchendolo progressivamente.

---

## Fase 1 – Parco mezzi minimale

1.1. Il sistema gestisce un parco mezzi.
1.2. In questa fase, un mezzo è indistinguibile dagli altri.
1.3. Il parco può essere inizialmente vuoto.
1.4. È possibile aggiungere un mezzo al parco.
1.5. È possibile rimuovere un mezzo dal parco.
1.6. Il sistema può sempre fornire il numero totale di mezzi presenti.

---

## Fase 2 – Disponibilità

2.1. Un mezzo può essere **disponibile** o **noleggiato**.
2.2. Noleggiare un mezzo disponibile lo rende non disponibile.
2.3. Non è possibile noleggiare un mezzo già noleggiato.
2.4. È possibile restituire un mezzo noleggiato.
2.5. In ogni momento è possibile conoscere il numero di mezzi disponibili.

---

## Fase 3 – Richieste e batch

3.1. Il sistema può ricevere un batch di richieste.
3.2. Un batch viene soddisfatto solo se **tutte** le richieste possono essere soddisfatte.
3.3. Se anche una sola richiesta fallisce, nessuna viene applicata.

---

## Fase 4 – Tipologie e dimensioni

4.1. I mezzi iniziano a differenziarsi per **dimensione**.
4.2. Ogni mezzo ha un numero di posti.
4.3. Una richiesta specifica il numero minimo di posti richiesti.
4.4. Un mezzo può soddisfare una richiesta solo se ha posti
sufficienti.
4.5. Una richiesta viene sempre assegnata a un solo mezzo.

---

## Fase 5 – Scelta del mezzo

5.1. Se più mezzi possono soddisfare una richiesta, il sistema sceglie quello con il minor numero di posti in eccesso.
5.2. L’assegnazione delle richieste non dipende dall’ordine di arrivo.
5.3. A parità di soluzioni valide, il risultato è deterministico.

---

## Fase 6 – Prezzi base

6.1. Ogni mezzo ha un costo base giornaliero.
6.2. Il costo di una prenotazione dipende dal mezzo.
6.3. Mezzi più grandi non possono costare meno di mezzi più piccoli.
6.4. Il costo totale di un batch è la somma dei costi delle singole prenotazioni.

---

## Fase 7 – Clienti e sconti

29. Ogni prenotazione è associata a un cliente.
30. Se un cliente prenota più mezzi nello stesso batch, ottiene uno sconto percentuale.
31. Lo sconto si applica al totale del cliente, non ai singoli mezzi.
32. Il prezzo finale non può mai essere negativo.

---

## Fase 8 – Profitto e ottimizzazione

33. Un batch di richieste genera un profitto totale.
34. Se esistono più assegnazioni valide, il sistema sceglie quella a profitto massimo.
35. Uno sconto può rendere una soluzione valida ma meno profittevole di un’altra.
36. Il sistema privilegia sempre il profitto totale rispetto alla soddisfazione di singole richieste.

---

## Fase 9 – Estensioni opzionali

37. Una prenotazione può includere servizi aggiuntivi a costo fisso.
38. I servizi aggiuntivi non sono soggetti a sconti.
39. Il carburante mancante alla restituzione viene addebitato al cliente.
40. Nessuna regola di business può violare la consistenza dello stato.

---

## Fase 10 - Requisiti perturbatori

- Alcuni mezzi sono premium e possono essere noleggiati solo pagando un sovrapprezzo fisso.

- I mezzi premium non partecipano agli sconti, anche se prenotati in batch.

- Un cliente può avere al massimo un mezzo attivo alla volta, indipendentemente dal tipo.

- È possibile prenotare più mezzi per un singolo evento, ma devono avere tutti la stessa durata.

- Il sistema può rifiutare un batch profittevole se non rispetta una politica di equità tra clienti.

- Alcuni clienti hanno un budget massimo che non può essere superato.

- Se una prenotazione supera il budget del cliente, l’intero batch fallisce.

- Il prezzo finale viene arrotondato per eccesso all’unità monetaria.

- I mezzi possono essere prenotati in anticipo e diventano indisponibili solo a partire da una certa data.

- Una prenotazione anticipata può essere annullata, ma prevede una penale fissa.
