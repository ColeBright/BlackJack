# Blackjack

An ASP.NET MVC rendition of the classic Blackjack game, also known as 21.

## What is Blackjack? How do I play?

Using a traditional deck of playing cards, a dealer deals you and themselves two initial cards. Face cards (King, Queen, and Jack) are worth 10. Numbered cards 2-10 are their original values. Aces are trickier - They are 11 if they don't put you over 21, at which point they are worth 1.
Speaking of 21, the goal is to have a hand with a higher value while not exceeding 21 in total. The ideal hand is something like an Ace and a face card or a ten card. If you are dealt a hand below that and would like to have another card to get closer to 21, you request a "Hit". But be careful - if you exceed 21 you lose the round. If you don't want any more cards, you "Stand" with your cards. Then dealer then reveals their covered card and it is determined then who wins.

## How to Use

1. **Build and run the application:**
   ```bash
   dotnet build
   dotnet run
   ```
 2. **Press "Play"**
 3. **Press "Hit" if you want to be dealt another card**
 4. **Press "Stand" if you want to remain with your current hand**
 5. **Press "New Round" if you want to start a new round**
