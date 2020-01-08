![platform](https://img.shields.io/badge/platform-.NET%20Core%202.2-blue?style=for-the-badge&logo=windows)
![version](https://img.shields.io/badge/version-1.0.0-yellow?style=for-the-badge&logo=visual-studio)


# MovieApi Project

`MovieApi` is a Sample Web API Project developed by [Birju Nakrani](https://github.com/birjunakrani), using **.Net Core 2.2** framework, **SQL server** as Database and **EntityFramework**. 

## Usage
It exhibits CRUD operation performed through Web API.

You can use any API testing tool like [_**postman**_](https://www.getpostman.com/) to test these API operations.

## ER Diagram & API Call Instructions
<img src="images/MovieApiProject_ER_diagram.jpg" width="450" align="right" />

Please visit [_**lucidchart**_](https://www.lucidchart.com/documents/view/5f949dd9-35c3-4d6c-80af-716b05ba4bdd/0_0) to see ER diagram showing entity relationships in detail. 

Next to the diagram, you can also find some _instructions_ to perform CRUD operation and _URLs_ to test these operations.

## Development
* Used `Code First` approach to map entity classes to Database tables using **EntityFramework**.

* I have used an instance of **SQL server** database, the connection string can be found in `launchsettings.json` file.

* I have also seeded initial data through _Database Seeding method_ for easy testing. 

## Things to bear in mind while testing
* If you are **Creating/Adding** data to some of the entities, you need to be mindful of relationship.

**e.g.** if you want to test POST method to add a `Director`, it also requires `Country` object to be associated with

If you are trying add a new `Director` using **POST** request with `localhost:xxx/api/directors` url, this is what you should include in the body of the request.
```
    {
        "firstName": "Paul",
        "lastName": "Anderson",
        "Country":{
                    "Id":xx,
                    "Name": "xxxx"
    }
```

Same way, if you want to **POST** a new `Review`, body requires `Critic` to be associated with due to parent-child relationship of the entities.
```
    {
        "headline": "xxxxxxxxx",    // please see the model class to see validation on these properties
        "reviewMovie": "xxxxxxxxxxxxxxxxxxxxxxxxxx.",   // please see the model class to see validation on these properties
        "rating": 4,
        "Critic" : 
        {
            "Id":xx,
            "FirstName": "xxx",
            "LastName":"xxxx"
        }
    }
```

* Some entity classes do not accept duplicated records, such as `Country`, `Category`.
* ALso, API is mindful of referential integrity, in other words, you can only delete a `Country` or `Category` if there are no `Director` or `Movie` associated with that instance.
* If you delete a `Critic`, all `Review` related to that CriticID will also be deleted. 

## Author
* **Birju Nakrani** - [GitHub](https://github.com/birjunakrani)
