# GroceryApp sprint4

## Gitflow
In dit project werken we met Gitflow.
Hieronder leg ik de verschillende branches uit en hoe we hiermee werken.

### Main
De Main branch is de stabiele versie van de app. Alleen releases en hotfixes komen hier terrecht.

### Develop
De Develop branch is de integratie branch waar alle features in worden samengevoegd.

### Feature
Voor elke user story (UC) wordt een aparte feature branch gemaakt vanuit Develop.
Zoals: 
- feature/UC10
- feature/UC13

### Release
Wanneer alle features voor een release klaar zijn, wordt er een release branch gemaakt vanuit Develop.
Zoals:
- release/v1.1.0

### Hotfix
Wanneer er een bug in de Main branch wordt gevonden, wordt er een hotfix branch gemaakt vanuit Main.
Zoals:
- hotfix/v1.0.1
# GroceryApp sprint3 Studentversie  

## UC10 Productaantal in boodschappenlijst
Aanpassingen zijn compleet.

## UC11 Meest verkochte producten
Vereist aanvulling:  
- Werk in GroceryListItemsService de methode GetBestSellingProducts uit.  
- In BestSellingProductsView de kop van de tabel aanvullen met de gewenste kopregel boven de tabel. Daarnaast de inhoud van de tabel uitwerken.

## UC13 Klanten tonen per product  
Deze UC toont de klanten die een bepaald product hebben gekocht:  
- Maak enum Role met als waarden None en Admin.  
- Geef de Client class een property Role metb als type de enum Role. De default waarde is None.  
- In Client Repo koppel je de rol Role.Admin aan user3 (= admin).
- In BoughtProductsService werk je de Get(productid) functie uit zodat alle Clients die product met productid hebben gekocht met client, boodschappenlijst en product in de lijst staan die wordt geretourneerd.  
- In BoughtProductsView moet de naam van de Client ewn de naam van de Boodschappenlijst worden getoond in de CollectionView.  
- In BoughtProductsViewModel de OnSelectedProductChanged uitwerken zodat bij een ander product de lijst correct wordt gevuld.  
- In GroceryListViewModel maak je de methode ShowBoughtProducts(). Als de Client de rol admin heeft dan navigeer je naar BoughtProductsView. Anders doe je niets.  
- In GroceryListView voeg je een ToolbarItem toe met als binding Client.Name en als Command ShowBoughtProducts.  


  
