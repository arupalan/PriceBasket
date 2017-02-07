# PriceBasket 
Price a basket of goods taking into account some special offers. This is a **realtime** multi-threaded basket pricer. Realtime means that this system provides verbs / api such that admin team located at a diferent location can add or update the system with new price economics or add a new product, and the system will immediately take the changes into consideration for a seller who is located at a different location for the basket to be correctly priced using active current economics; without requiring a system restart for the change to take effect. 

An Example Price Economics 
  * Soup – 65p per tin
  * Bread – 80p per loaf
  * Milk – £1.30 per bottle
  * Apples – £1.00 per bag
  
Example special offers:
  * Apples have a 10% discount off their normal price this week
  * Buy 2 tins of soup and get a loaf of bread for half price

# System Usage
1. Start the system and issue command verbs. The system takes json as datastream.

  Commands verbs supported are
~~~
pricebasket     verb to price a basket of Items. This shows basket Subtotal , discount and Total

puteconomics    verb to add or update basket item economics.

quit            verb to quit
~~~  
You can then price as below which will show output as shown below

~~~
PriceBasket.Service.exe pricebasket --basket "[{'Name':'Apple', 'Unit':0},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':1}]"

pricebasket --basket "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':1}]"
~~~

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
