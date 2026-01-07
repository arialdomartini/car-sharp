## Contesto

Stai sviluppando il core di un **servizio di car renting**.
Il sistema non ha interfaccia grafica: è una libreria che gestisce **stato**, **regole di business** e **decisioni automatiche**.
Il dominio viene introdotto **per passi**, partendo da un modello volutamente povero e arricchendolo progressivamente.

---

## Fase 1 – Parco mezzi minimale

- Il sistema gestisce un parco mezzi.
- In questa fase, un mezzo è indistinguibile dagli altri.
- Il parco può essere inizialmente vuoto.
- È possibile aggiungere un mezzo al parco.
- È possibile rimuovere un mezzo dal parco.
- Il sistema può sempre fornire il numero totale di mezzi presenti.

---

## Fase 2 – Disponibilità

- Un mezzo può essere **disponibile** o **noleggiato**.
- Noleggiare un mezzo disponibile lo rende non disponibile.
- Non è possibile noleggiare un mezzo già noleggiato.
- È possibile restituire un mezzo noleggiato.
- In ogni momento è possibile conoscere il numero di mezzi disponibili.

---

## Fase 3 – Richieste e batch

- Il sistema può ricevere un batch di richieste.
- Un batch viene soddisfatto solo se **tutte** le richieste possono essere soddisfatte.
- Se anche una sola richiesta fallisce, nessuna viene applicata.

---

## Fase 4 – Tipologie e dimensioni

- I mezzi iniziano a differenziarsi per **dimensione**.
- Ogni mezzo ha un numero di posti.
- Una richiesta specifica il numero minimo di posti richiesti.
- Un mezzo può soddisfare una richiesta solo se ha posti
sufficienti.
- Una richiesta viene sempre assegnata a un solo mezzo.

---

## Fase 5 – Scelta del mezzo

- Se più mezzi possono soddisfare una richiesta, il sistema sceglie quello con il minor numero di posti in eccesso.
- L’assegnazione delle richieste non dipende dall’ordine di arrivo.
- A parità di soluzioni valide, il risultato è deterministico.

---

## Fase 6 – Prezzi base

- Ogni mezzo ha un costo base giornaliero.
- Il costo di una prenotazione dipende dal mezzo.
- Mezzi più grandi non possono costare meno di mezzi più piccoli.
- Il costo totale di un batch è la somma dei costi delle singole prenotazioni.

---

## Fase 7 – Clienti e sconti

- Ogni prenotazione è associata a un cliente.
- Se un cliente prenota più mezzi nello stesso batch, ottiene uno sconto percentuale.
- Lo sconto si applica al totale del cliente, non ai singoli mezzi.
- Il prezzo finale non può mai essere negativo.

---

## Fase 8 – Profitto e ottimizzazione

- Un batch di richieste genera un profitto totale.
- Se esistono più assegnazioni valide, il sistema sceglie quella a profitto massimo.
- Uno sconto può rendere una soluzione valida ma meno profittevole di un’altra.
- Il sistema privilegia sempre il profitto totale rispetto alla soddisfazione di singole richieste.

---

## Fase 9 – Estensioni opzionali

- Una prenotazione può includere servizi aggiuntivi a costo fisso.
- I servizi aggiuntivi non sono soggetti a sconti.
- Il carburante mancante alla restituzione viene addebitato al cliente.
- Nessuna regola di business può violare la consistenza dello stato.

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
