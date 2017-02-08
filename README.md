# PriceBasket 
Price a basket of goods taking into account some special offers. This is a **realtime** multi-threaded basket pricer. *Realtime* implies that this system provides verbs / api such that admin team seated at a diferent location can add or update the system with new price economics or add a new product, and the system will immediately take the changes into consideration for a seller who is located at a completely different location. The system will price the basket correctly using active current economics; without requiring a system restart for the change to take effect. 

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

  Commands verbs currently supported are
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
 
# Example pricing multipack discount
~~~
pricebasket --basket "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':1},{'Name':'Soup', 'Unit':2}]"
~~~

 ![Console Mode](http://www.alanaamy.net/wp-content/uploads/2017/02/ExampleMultiPackDiscount.png)
 
# Example Adding a new product economics
 You will get exception when you price a product with non-existent economics. Eg oil is currently not setup so we get exception. We then put the oil economics and we can now price Oil again without requiring a system restart.
 
~~~
pricebasket --basket "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0},{'Name':'Oil', 'Unit':0}]"
puteconomics --itemeconomics "[{'Name':'Oil','Price':0.95,'Discount':0.1}]"
pricebasket --basket "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0},{'Name':'Oil', 'Unit':1}]"
~~~

 ![Console Mode](http://www.alanaamy.net/wp-content/uploads/2017/02/AddNewProduct.png)

# Pre-requisites and Building From Source
* .Net Framework 4.6.1
* Visual Studio 2015 if you prefer IDE or of you may use git console and msbuild
* Access to nuget repository
* Xunit and Moq for testing framework
* Autofac for Dependency Injection
* CommandLineParser for verbs
* log4net for logging
* Newtonsoft.Json for json support
* xunit.runner.visualstudio if you prefer visual studio for test outputs

# System Architecture Diagram

![Console Mode](http://www.alanaamy.net/wp-content/uploads/2017/02/pricebasket.jpg)

## Modules Components
* BasketPricer
* BasketEconomicsManager
* BasketPriceReporter
* CommandProcessor
* LoggingModule (Extension of AutofacModule to facilitate Ilog injection )
* DependencyInjector (Autofac Injection Helper)

### Models
* **Basket**  *Encapsulates the list of items in the basket. The basket SubTotal, and the Total and the constituent items. Note the properties are readonly so that once a basket is created the computed prices cannot be changed which is specific to the constituents of the basket. Note that the basket is immutable.*

* **BasketItemEconomics** *Encapsulates a product economics like Price , Discount and MultiPackDiscount. Note the IEqualityComparer uses the Name field. In production implementation it will be useful to have additional identified like barcode sku etc. For simplicity of json; it is as it is but will be advantageous to have the economics as readonly. You can leverage json schema validation and a factory to provide immutability.*

* **MultiDiscount** *Encapsulates Discount which is active only at basket level. ItemName is the name of another item whose number of unit has to match to be equal or greater than at the basket level for this Discount to be applied*

* **BasketRequestItem** *Encapsulates a BasketRequestItem which is requested to be added to the basket. For simplicity of json; it is as it is but will be advantageous to have the Name and Unit as readonly. You can leverage json schema validation and a factory to provide this immutability.*

# Source code struture. Feature based folder structure

 ![Console Mode](http://www.alanaamy.net/wp-content/uploads/2017/02/pricebasketfolders.png)
 
# Xunit Unit Tests using Re-sharper Console
 ![Console Mode](http://www.alanaamy.net/wp-content/uploads/2017/02/pricebasketUnit-tests.png)
