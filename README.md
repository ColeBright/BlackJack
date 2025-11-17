# Blackjack

An **ASP.NET MVC** rendition of the classic Blackjack card game (also known as 21).

---

## Features

- Single-player game against a dealer
- Accurate Blackjack rules with card values:
  - Number cards (2–10) = face value
  - Face cards (Jack, Queen, King) = 10
  - Ace = 11 or 1 depending on hand
- "Hit" and "Stand" actions for player
- Automatic dealer play according to standard rules
- Detects natural Blackjack and Pushes
- Start a new round at any time

---

## What is Blackjack?

Blackjack is played with a standard deck of cards. The goal is to have a hand **closer to 21 than the dealer** without exceeding it.

### Card Values

- Number cards (2–10) = face value  
- Face cards (Jack, Queen, King) = 10  
- Ace = 11, unless it would make your hand exceed 21, then 1  

### Gameplay

1. Both player and dealer are dealt **two initial cards**.
2. If you want another card, you **"Hit"**.
3. If you are satisfied with your hand, you **"Stand"**.
4. The dealer reveals their hidden card and plays according to standard rules.
5. The round is won by the player with the hand **closest to 21 without going over**.

### Special Cases

- A starting hand of an Ace and a 10-value card = **Blackjack** (instant win unless dealer also has it).

---

## How to Play

1. **Build and run the application:**
   ```bash
   dotnet build
   dotnet run
 2. **Press "Play"**
 3. **Press "Hit" if you want to be dealt another card**
 4. **Press "Stand" if you want to remain with your current hand**
 5. **Press "New Round" if you want to start a new round**
