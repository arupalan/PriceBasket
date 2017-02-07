# PriceBasket
Price a basket of goods taking into account some special offers.

An Example Price Economics 
  * Soup – 65p per tin
  * Bread – 80p per loaf
  * Milk – £1.30 per bottle
  * Apples – £1.00 per bag
  
Current special offers:
  * Apples have a 10% discount off their normal price this week
  * Buy 2 tins of soup and get a loaf of bread for half price

# System Usage
1. Start the system and issue command verbs which takes json data stream
  Commands verbs supported are
  pricebasket     verb to price a basket of Items. This shows basket Subtotal ,
                  discount and Total

  puteconomics    verb to add or update basket item economics.

  quit            verb to quit
  
You can then price as below which will show output as shown below
 ![Console Mode](http://www.alanaamy.net/wp-content/uploads/2017/02/PriceBasket-General-Use.png)
# Building From Source
1. Move to your local git repository directory or any directory (with git init) in console.

2. Clone repository.

        git clone https://github.com/arupalan/RxTimedWindow.git

3. Move to source directory, update submodule and build.

        cd AlanAamy.Net.RxTimedWindow/
        git submodule update --init --recursive
        msbuild
        
#Installation

 * installutil /LogFile=svcinstalllog.txt AlanAamy.Net.RxTimedWindow.Service.exe
 
 #Debug 
 * You can execute with switch -console to see the logs on console
 ![Console Mode](http://www.alanaamy.net/wp-content/uploads/2015/07/RxTimedWindowErrors.png)
 
 #Diagnostics
 * The diagnostics snapshot of performance
  ![Diagnostic Mode](http://www.alanaamy.net/wp-content/uploads/2015/07/RxTimedWindowTestsMemory.png)
