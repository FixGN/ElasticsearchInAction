using ElasticsearchInAction.Api.Models;
using ElasticsearchInAction.Api.Repositories;
using ElasticsearchInAction.Api.Responses;
using Dto = ElasticsearchInAction.Repositories.Elasticsearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchInAction.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SetupController : ControllerBase
{
    [HttpGet("Books")]
    public async Task<dynamic> SetupBooks(
        [FromServices] IElasticRepository repository,
        CancellationToken cancellationToken)
    {
        var response = await repository.BulkSave(GetDefaultBooks(), cancellationToken);

        return new
        {
            Ids = response
        };
    }

    private static Dto.Book[] GetDefaultBooks()
    {
        return new[]
        {
            new Dto.Book
            {
                Title = "Core Java Volume I: Fundamentals",
                Author = "Cay S. Horstmann",
                Edition = 11,
                Synopsys =
                    "Java reference book that offers a detailed explanation of various features of Core Java, including exception handling, interfaces, and lambda expressions. Significant highlights of the book include simple language, conciseness, and detailed examples.",
                AmazonRating = 4.6,
                ReleaseDate = new DateOnly(2018, 08, 27),
                Tags = new[] { "Programming Languages, Java Programming" },
                BestSeller = false,
                Prices = new Dictionary<string, double>
                {
                    { "usd", 12 },
                    { "eur", 10 },
                    { "gbp", 9 }
                }
            },
            new Dto.Book
            {
                Title = "Effective Java",
                Author = "Joshua Bloch",
                Edition = 3,
                Synopsys =
                    "A must-have book for every Java programmer and Java aspirant, Effective Java makes up for an excellent complementary read with other Java books or learning material. The book offers 78 best practices to follow for making the code better.",
                AmazonRating = 4.7,
                ReleaseDate = new DateOnly(2017, 12, 27),
                Tags = new[] { "Object Oriented Software Design" },
                BestSeller = true,
                Prices = new Dictionary<string, double>
                {
                    { "usd", 21 },
                    { "eur", 18 },
                    { "gbp", 16 }
                }
            },
            new Dto.Book
            {
                Title = "Java: A Beginner's Guide",
                Author = "Herbert Schildt",
                Edition = 8,
                Synopsys =
                    "One of the most comprehensive books for learning Java. The book offers several hands-on exercises as well as a quiz section at the end of every chapter to let the readers self-evaluate their learning.",
                AmazonRating = 4.2,
                ReleaseDate = new DateOnly(2018, 11, 20),
                Tags = new[] { "Software Design & Engineering", "Internet & Web" },
                BestSeller = true,
                Prices = new Dictionary<string, double>
                {
                    { "usd", 12 },
                    { "eur", 12 },
                    { "gbp", 11 }
                }
            }
        };
    }
}


/*{"index":{"_index":"books","_id":"4"}}
{"title": "Java - The Complete Reference","author": "Herbert Schildt","edition": 11,"synopsis": "Convenient Java reference book examining essential portions of the Java API library, Java. The book is full of discussions and apt examples to better Java learning.","amazon_rating": 4.4,"release_date": "2019-03-19","tags": ["Software Design & Engineering", "Internet & Web", "Computer Programming Language & Tool"]}
{"index":{"_index":"books","_id":"5"}}
{"title": "Head First Java","author": "Kathy Sierra and Bert Bates","edition":2, "synopsis": "The most important selling points of Head First Java is its simplicity and super-effective real-life analogies that pertain to the Java programming concepts.","amazon_rating": 4.3,"release_date": "2005-02-18","tags": ["IT Certification Exams", "Object-Oriented Software Design","Design Pattern Programming"]}
{"index":{"_index":"books","_id":"6"}}
{"title": "Java Concurrency in Practice","author": "Brian Goetz with Tim Peierls, Joshua Bloch, Joseph Bowbeer, David Holmes, and Doug Lea","edition": 1,"synopsis": "Java Concurrency in Practice is one of the best Java programming books to develop a rich understanding of concurrency and multithreading.","amazon_rating": 4.3,"release_date": "2006-05-09","tags": ["Computer Science Books", "Programming Languages", "Java Programming"]}
{"index":{"_index":"books","_id":"7"}}
{"title": "Test-Driven: TDD and Acceptance TDD for Java Developers","author": "Lasse Koskela","edition": 1,"synopsis": "Test-Driven is an excellent book for learning how to write unique automation testing programs. It is a must-have book for those Java developers that prioritize code quality as well as have a knack for writing unit, integration, and automation tests.","amazon_rating": 4.1,"release_date": "2007-10-22","tags": ["Software Architecture", "Software Design & Engineering", "Java Programming"]}
 {"index":{"_index":"books","_id":"8"}}
 {"title": "Head First Object-Oriented Analysis Design","author": "Brett D. McLaughlin, Gary Pollice & David West","edition": 1,"synopsis": "Head First is one of the most beautiful finest book series ever written on Java programming language. Another gem in the series is the Head First Object-Oriented Analysis Design.","amazon_rating": 3.9,"release_date": "2014-04-29","tags": ["Introductory & Beginning Programming", "Object-Oriented Software Design", "Java Programming"]}
 {"index":{"_index":"books","_id":"9"}}
 {"title": "Java Performance: The Definite Guide","author": "Scott Oaks","edition": 1,"synopsis": "Garbage collection, JVM, and performance tuning are some of the most favorable aspects of the Java programming language. It educates readers about maximizing Java threading and synchronization performance features, improve Java-driven database application performance, tackle performance issues","amazon_rating": 4.1,"release_date": "2014-03-04","tags": ["Design Pattern Programming", "Object-Oriented Software Design", "Computer Programming Language & Tool"]}
 {"index":{"_index":"books","_id":"10"}}
 {"title": "Head First Design Patterns", "author": "Eric Freeman & Elisabeth Robson with Kathy Sierra & Bert Bates","edition": 10,"synopsis": "Head First Design Patterns is one of the leading books to build that particular understanding of the Java programming language." ,"amazon_rating": 4.5,"release_date": "2014-03-04","tags": ["Design Pattern Programming", "Object-Oriented Software Design eTextbooks", "Web Development & Design eTextbooks"]}
 {"index":{"_index":"books","_id":"11"}}
 {"title": "JavaScript - The Definitive Guide", "author": "David Flanagan","edition": 1,"synopsis": "JavaScript is the programming language of the web and is used by more software developers today than any other programming language. For nearly 25 years this best seller has been the go-to guide for JavaScript programmers." ,"amazon_rating": 4.7,"release_date": "2020-05-29","tags": ["Design Pattern Programming", "Object-Oriented Software Design eTextbooks", "Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"12"}}
 {"title": "Eloquent Javascript", "author": "Marijn Haverbeke","edition": 3,"synopsis": "JavaScript lies at the heart of almost every modern web application, from social apps like Twitter to browser-based game frameworks like Phaser and Babylon. Though simple for beginners to pick up and play with, JavaScript is a flexible, complex language that you can use to build full-scale applications." ,"amazon_rating": 4.6,"release_date": "2018-12-14","tags": ["DOM", "Node.js","object-oriented and functional programming techniques", "Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"13"}}
 {"title": "JavaScript and JQuery: Interactive Front–End Web", "author": "Jon Duckett","edition": 1,"synopsis": "This full-color book adopts a visual approach to teaching JavaScript & jQuery, showing you how to make web pages more interactive and interfaces more intuitive through the use of inspiring code examples, infographics, and photography." ,"amazon_rating": 4.7,"release_date": "2014-07-18","tags": ["DJavaScript and jQuery", "JavaScript APIs, and jQuery plugins", "Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"14"}}
 {"title": "A Smarter Way to Learn JavaScript", "author": "Mark Myers","edition": 1,"synopsis": "JThe first problem is retention. You remember only ten or twenty percent of what you read. That spells failure. To become fluent in a computer language, you have to retain pretty much everything." ,"amazon_rating": 4.6,"release_date": "2014-03-20","tags": ["Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"15"}}
 {"title": "Head First JavaScript Programming", "author": "Eric T. Freeman and Elisabeth Robson","edition": 1,"synopsis": "This brain-friendly guide teaches you everything from JavaScript language fundamentals to advanced topics, including objects, functions, and the browser’s document object model." ,"amazon_rating": 4.5,"release_date": "2014-04-10","tags": ["The secrets of JavaScript types", "The inner details of JavaScript", "Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"16"}}
 {"title": "Modern JavaScript for the Impatient", "author": "Cay Horstmann","edition": 1,"synopsis": "Modern JavaScript for the Impatient is a complete yet concise guide to JavaScript E6 and beyond. Rather than first requiring you to learn and transition from older versions, it helps you quickly get productive with today’s far more powerful versions and rapidly move from languages such as Java, C#, C, or C++." ,"amazon_rating": 4.9,"release_date": "2020-08-18","tags": ["modern JavaScript", "Object-Oriented Programming", "JavaScript libraries, frameworks, and platforms"]}
 {"index":{"_index":"books","_id":"17"}}
 {"title": "JavaScript in easy steps", "author": "Mike McGrath","edition": 6,"synopsis": "JavaScript in easy steps, 6th edition instructs the user how to create exciting web pages that employ the power of JavaScript to provide functionality. You need have no previous knowledge of any scripting language so it's ideal for the newcomer to JavaScript. By the end of this book you will have gained a sound understanding of JavaScript and be able to add exciting dynamic scripts to your own web pages." ,"amazon_rating": 4.4,"release_date": "2020-02-28","tags": ["Get Started in JavaScript", "Interact with the Document", "Create Web Applications"]}
 {"index":{"_index":"books","_id":"18"}}
 {"title": "JavaScript: The Good Parts", "author": "Douglas Crockford","edition": 1,"synopsis": "Most programming languages contain good and bad parts, but JavaScript has more than its share of the bad, having been developed and released in a hurry before it could be refined. This authoritative book scrapes away these bad features to reveal a subset of JavaScript that's more reliable, readable, and maintainable than the language as a whole—a subset you can use to create truly extensible and efficient code." ,"amazon_rating": 4.5,"release_date": "2008-05-18","tags": ["Design Pattern Programming", "Object-Oriented Programming", "Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"19"}}
 {"title": "JavaScript Everywhere", "author": "Adam Scott","edition": 1,"synopsis": "JavaScript is the little scripting language that could. Once used chiefly to add interactivity to web browser windows, JavaScript is now a primary building block of powerful and robust applications. In this practical book, new and experienced JavaScript developers will learn how to use this language to create APIs as well as web, mobile, and desktop applications." ,"amazon_rating": 4.5,"release_date": "2020-02-21","tags": ["Building Cross-platform Applications with GraphQL, React, React Native, and Electron", "Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"20"}}
 {"title": "JavaScript The Complete Reference", "author": "Thomas A. Powell and Fritz Schneider","edition": 3,"synopsis": "Design, debug, and publish high-performance web pages and applications using tested techniques and best practices from expert developers. The all-new edition of this comprehensive guide has been thoroughly revised and expanded to cover the latest JavaScript features, tools, and programming methods." ,"amazon_rating": 4.3,"release_date": "2012-09-16","tags": ["XMLHttpRequest object to create Ajax applications", "DOM", "Programming Languages, Javascript Programming"]}
 {"index":{"_index":"books","_id":"21"}}
 {"title": "The C# Player's Guide", "author": "RB Whitaker","edition": 5,"synopsis": "The book in your hands is a different kind of programming book. Like an entertaining video game, programming is an often challenging but always rewarding experience. This book shakes off the dusty, dull, dryness of the typical programming book, replacing it with something more exciting and flavorful: a bit of humor, a casual tone, and examples involving dragons and asteroids instead of bank accounts and employees." ,"amazon_rating": 5.0,"release_date": "2022-01-14","tags": ["basic mechanics of C#", "object-oriented programming", "advanced C# features"]}
 {"index":{"_index":"books","_id":"22"}}
 {"title": "C# 10 and .NET 6", "author": "Mark J. Price","edition": 1,"synopsis": "You will learn object-oriented programming, writing, testing, and debugging functions, implementing interfaces, and inheriting classes. The book covers the .NET APIs for performing tasks like managing and querying data, monitoring and improving performance, and working with the filesystem, async streams, serialization, and encryption. It provides examples of cross-platform apps you can build and deploy, such as websites and services using ASP.NET Core." ,"amazon_rating": 4.6,"release_date": "2021-11-09","tags": ["real-world applications", "latest features of C# 10 and .NET 6", "Visual Studio 2022 and Visual Studio Code"]}
 {"index":{"_index":"books","_id":"23"}}
 {"title": "C# Programming in easy steps", "author": "Mike McGrath","edition": 2,"synopsis": "C# Programming in easy steps, 2nd edition will teach you to code applications, and demonstrates every aspect of the C# language you will need to produce professional programming results. Its examples provide clear syntax-highlighted code showing C# language basics including variables, arrays, logic, looping, methods, and classes." ,"amazon_rating": 4.6,"release_date": "2020-05-29","tags": ["programming in C#", "creating apps", "fundamental understanding of C#"]}
 {"index":{"_index":"books","_id":"24"}}
 {"title": "Professional C# and .NET", "author": "Christian Nagel","edition": 1,"synopsis": "Experienced programmers making the transition to C# will benefit from the author’s in-depth explorations to create Web- and Windows applications using ASP.NET Core, Blazor, and WinUI using modern application patterns and new features offered by .NET including Microservices deployed to Docker images, GRPC, localization, asynchronous streaming, and much more." ,"amazon_rating": 4.5,"release_date": "2022-03-04","tags": ["extension of .NET to non-Microsoft platforms like OSX and Linux", "Microsoft Azure services such as Azure App", "C# 10 and .NET 6"]}
 {"index":{"_index":"books","_id":"25"}}
 {"title": "Head First C#", "author": "Stellman, Andrew, Greene and Jennifer","edition": 4,"synopsis": "What will you learn from this book? For beginning programmers looking to learn C#, this practical guide provides a bright alternative to the legions of dull tutorials on this popular object-oriented language." ,"amazon_rating": 4.6,"release_date": "2021-01-29","tags": ["Real-World Programming with C# and .Net Core", "Object-oriented language", "multi-sensory learning"]}
 {"index":{"_index":"books","_id":"26"}}
 {"title": "C# 9.0 in a Nutshell", "author": "Albahari and Joseph","edition": 1,"synopsis": "When you have questions about C# 9.0 or .NET 5, this bestselling guide has the answers. C# is a language of unusual flexibility and breadth, but with its continual growth, there's so much more to learn. In the tradition of O'Reilly's Nutshell guides, this thoroughly updated edition is simply the best one-volume reference to the C# language available today." ,"amazon_rating": 4.5,"release_date": "2021-03-31","tags": ["C# and .NET", "pointers, closures, and patterns Dig deep into LINQ", "Programming Languages"]}
 {"index":{"_index":"books","_id":"27"}}
 {"title": "C# in Depth", "author": "Jon Skeet","edition": 4,"synopsis": "C# is an amazing language that's up to any challenge you can throw at it. As a C# developer, you also need to be up to the task." ,"amazon_rating": 4.6,"release_date": "2019-05-20","tags": ["C# 5, 6, and 7", "better code with tuples, string interpolation, pattern matching"]}
 {"index":{"_index":"books","_id":"28"}}
 {"title": "C# Data Structures and Algorithms", "author": "Marcin Jamro","edition": 1,"synopsis": "Data structures allow organizing data efficiently. They are critical to various problems and their suitable implementation can provide a complete solution that acts like reusable code. In this book, you will learn how to use various data structures while developing in the C# language as well as how to implement some of the most common algorithms used with such data structures." ,"amazon_rating": 4.6,"release_date": "2018-04-19","tags": ["Implement algorithms", "Build enhanced applications by using hashtables, dictionaries and sets", "Programming Languages, C#"]}
 {"index":{"_index":"books","_id":"29"}}
 {"title": "Learning C# by Developing Games with Unity 3D", "author": "Terry Norton","edition": 1,"synopsis": "For the absolute beginner to any concept of programming, writing a script can appear to be an impossible hurdle to overcome. The truth is, there are only three simple concepts to understand: 1) having some type of information; 2) using the information; and 3) communicating the information. Each of these concepts is very simple and extremely important. These three concepts are combined to access the feature set provided by Unity." ,"amazon_rating": 3.9,"release_date": "2013-09-25","tags": ["Unity C# scripts", "GameObjects and Component objects", "Unity's Scripting"]}
 {"index":{"_index":"books","_id":"30"}}
 {"title": "Pro C# 9 with .NET 5", "author": "Andrew Troelsen and Phillip Japikse","edition": 1,"synopsis": "This essential classic provides a comprehensive foundation in the C# programming language and the framework it lives in. Now in its 10th edition, you will find the latest C# 9 and .NET 5 features served up with plenty of "behind the curtain" discussion designed to expand developers’ critical thinking skills when it comes to their craft. Coverage of ASP.NET Core, Entity Framework Core, and more, sits alongside the latest updates to the new unified .NET platform, from performance improvements to Windows Desktop apps on .NET 5, updates in XAML tooling, and expanded coverage of data files and data handling. Going beyond the latest features in C# 9, all code samples are rewritten for this latest release." ,"amazon_rating": 4.7,"release_date": "2021-06-08","tags": ["C# 9 features", "ASP.NET Core web applications and web services", "C# and modern frameworks for services"]}
 {"index":{"_index":"books","_id":"31"}}
 {"title": "Python Crash Course", "author": "Eric Matthes","edition": 2,"synopsis": "Reading books is a kind of enjoyment. Reading books is a good habit. We bring you a different kinds of books. You can carry this book where ever you want. It is easy to carry. It can be an ideal gift to yourself and to your loved ones. Care instruction keep away from fire." ,"amazon_rating": 4.7,"release_date": "2019-05-09","tags": ["Software Architecture", "Functional Programming", "General Introduction to Programming"]}
 {"index":{"_index":"books","_id":"32"}}
 {"title": "Automate The Boring Stuff With Python", "author": "Al Sweigart","edition": 2,"synopsis": "Reading books is a kind of enjoyment. Reading books is a good habit. We bring you a different kinds of books. You can carry this book where ever you want. It is easy to carry. It can be an ideal gift to yourself and to your loved ones. Care instruction keep away from fire." ,"amazon_rating": 4.7,"release_date": "2019-10-17","tags": ["Software Architecture", "Functional Programming", "General Introduction to Programming"]}
 {"index":{"_index":"books","_id":"33"}}
 {"title": "Python Projects for Beginners", "author": "Connor P. Milliken","edition": 1,"synopsis": "Immerse yourself in learning Python and introductory data analytics with this book’s project-based approach. Through the structure of a ten-week coding bootcamp course, you’ll learn key concepts and gain hands-on experience through weekly projects." ,"amazon_rating": 4.7,"release_date": "2019-11-16","tags": ["Python language", "Python Data Analysis library", "Anaconda, Jupyter Notebooks, and the Python Shell"]}
 {"index":{"_index":"books","_id":"34"}}
 {"title": "Python All-in-One For Dummies", "author": "John C. Shovic and Alan Simpson","edition": 2,"synopsis": "Powerful and flexible, Python is one of the most popular programming languages in the world. It's got all the right stuff for the software driving the cutting-edge of the development world—machine learning, robotics, artificial intelligence, data science, etc. The good news is that it’s also pretty straightforward to learn, with a simplified syntax, natural-language flow, and an amazingly supportive user community." ,"amazon_rating": 4.3,"release_date": "2021-04-09","tags": ["Python Building Blocks", "Artificial Intelligence and Python", "Data Science and Python"]}
 {"index":{"_index":"books","_id":"35"}}
 {"title": "Learning Python", "author": "Mark Lutz ","edition": 1,"synopsis": "Get a comprehensive, in-depth introduction to the core Python language with this hands-on book. Based on author Mark Lutz’s popular training course, this updated fifth edition will help you quickly write efficient, high-quality code with Python. It’s an ideal way to begin, whether you’re new to programming or a professional developer versed in other languages." ,"amazon_rating": 4.5,"release_date": "2013-07-06","tags": ["Software quality", "Enjoyment", "Powerful Object-Oriented Programming"]}
 {"index":{"_index":"books","_id":"36"}}
 {"title": "Data Structure and Algorithmic Thinking with Python", "author": "Narasimha Karumanchi","edition": 1,"synopsis": ""Data Structure and Algorithmic Thinking with Python" is designed to give a jump-start to programmers, job hunters and those who are appearing for exams. All the code in this book are written in Python. It contains many programming puzzles that not only encourage analytical thinking, but also prepares readers for interviews. This book, with its focused and practical approach, can help readers quickly pick up the concepts and techniques for developing efficient and effective solutions to problems." ,"amazon_rating": 4.2,"release_date": "2015-01-29","tags": ["Algorithmic Programming", "Introduction to Programming", "Python Programming"]}
 {"index":{"_index":"books","_id":"37"}}
 {"title": "Python for Data Analysis", "author": "Wes Mckinney","edition": 2,"synopsis": "Get complete instructions for manipulating, processing, cleaning, and crunching datasets in Python. Updated for Python 3.6, the second edition of this hands-on guide is packed with practical case studies that show you how to solve a broad set of data analysis problems effectively. You’ll learn the latest versions of pandas, NumPy, IPython, and Jupyter in the process. " ,"amazon_rating": 4.6,"release_date": "2017-11-03","tags": ["pandas, NumPy, IPython, and Jupyter", "Python Programming", "Data Analytics"]}
 {"index":{"_index":"books","_id":"38"}}
 {"title": "Effective Pandas", "author": "Matt Harrison", "edition": 1, "synopsis": "Best practices for manipulating data with Pandas. This book will arm you with years of knowledge and experience that are condensed into an easy to follow format. Rather than taking months reading blogs and websites and searching mailing lists and groups, this book will teach you how to write good Pandas code." ,"amazon_rating": 4.9,"release_date": "2021-12-08","tags": ["Data Manipulation", "Visualization", "Programming Languages"]}
 {"index":{"_index":"books","_id":"39"}}
 {"title": "Python by Example", "author": "Terry Norton","edition": 1,"synopsis": "Python is today's fastest growing programming language. This engaging and refreshingly different guide breaks down the skills into clear step-by-step chunks and explains the theory using brief easy-to-understand language. Rather than bamboozling readers with pages of mind-numbing technical jargon, this book includes 150 practical challenges, putting the power in the reader's hands." ,"amazon_rating": 4.5,"release_date": "2019-06-06","tags": ["Introduction To Programming", "Python Programming"]}
 {"index":{"_index":"books","_id":"40"}}
 {"title": "Python Distilled", "author": "David Beazley","edition": 1,"synopsis": "The richness of modern Python challenges developers at all levels. How can programmers who are new to Python know where to begin without being overwhelmed? How can experienced Python developers know they're coding in a manner that is clear and effective? How does one make the jump from learning about individual features to thinking in Python at a deeper level? Dave Beazley's new Python Distilled addresses these and many other real-world issues." ,"amazon_rating": 4.7,"release_date": "2021-11-09","tags": ["Introduction To Programming", "Python Programming"]}
 {"index":{"_index":"books","_id":"41"}}
 {"title": "Kotlin Programming", "author": "Josh Skeen, Andrew Bailey and David Greenhalgh","edition": 1,"synopsis": "Kotlin is a statically typed programming language designed to interoperate with Java and fully supported by Google on the Android operating system. It is also a multiplatform language that can be used to write code that can be shared across platforms including macOS, iOS, Windows, and JavaScript." ,"amazon_rating": 5.0,"release_date": "2022-01-13","tags": ["Kotlin Essentials", "Kotlin concepts and foundational APIs"]}
v {"index":{"_index":"books","_id":"42"}}
 {"title": "Java to Kotlin", "author": "Duncan McGregor and Nat Pryce","edition": 1,"synopsis": "It takes a week to travel the 8,000 miles overland from Java to Kotlin. If you're an experienced Java developer who has tried the Kotlin language, you were probably productive in about the same time. " ,"amazon_rating": 5.0,"release_date": "2021-08-24","tags": ["Kotlin from scratch", "mixed language codebase"]}
 {"index":{"_index":"books","_id":"43"}}
 {"title": "Kotlin in Action", "author": "DDmitry Jemerov","edition": 1,"synopsis": "Kotlin is a new programming language targeting the Java platform. It offers on expressiveness and safety without compromising simplicity,seamless interoperability with existing Java code, and great tooling support. Because Kotlin generates regular Java bytecode and works together with existing Java libraries and frameworks, it can be used almost everywhere where Java is used today - for server-side development, Android apps, and much more." ,"amazon_rating": 4.7,"release_date": "2017-03-27","tags": ["IMobile Phone Programming", "Introduction to Programming
"]}
 {"index":{"_index":"books","_id":"44"}}
 {"title": "Head First Kotlin", "author": "Dawn Griffiths and David Griffiths","edition": 1,"synopsis": "This hands-on book helps you learn the Kotlin language with a unique method that goes beyond syntax and how-to manuals, and teaches you how to think like a great Kotlin developer. You ll learn everything from language fundamentals to collections, generics, lambdas, and higher-order functions." ,"amazon_rating": 4.4,"release_date": "2019-02-28","tags": ["Introduction To Programming", "Kotlin language"]}
 {"index":{"_index":"books","_id":"45"}}
 {"title": "Kotlin Cookbook", "author": "Ken Kousen","edition": 1,"synopsis": "Use Kotlin to build Android apps, web applications, and more—while you learn the nuances of this popular language. With this unique cookbook, developers will learn how to apply thisJava-based language to their own projects. Both experienced programmers and those new to Kotlin will benefit from the practical recipes in this book." ,"amazon_rating": 4.4,"release_date": "2019-11-22","tags": ["Introduction To Programming", "Mobile Phone Programming", "Android and Spring"]}
 {"index":{"_index":"books","_id":"46"}}
 {"title": "The Joy of Kotlin", "author": "Pierre-Yves Saumont Saumont","edition": 1,"synopsis": "The Joy of Kotlin teaches readers the right way to code in Kotlin. This insight-rich book covers everything needed to master the Kotlin language while exploring coding techniques that will make readers better developers no matter what language they use." ,"amazon_rating": 4.3,"release_date": "2019-05-20","tags": ["Introduction To Programming", "Safe handling of errors and exceptions", "Dealing with optional data"]}
 {"index":{"_index":"books","_id":"47"}}
 {"title": "Discovering Statistics Using R", "author": "Andy Field, Jeremy Miles and Zoe Field","edition": 1,"synopsis": "Discovering Statistics Using R takes students on a journey of statistical discovery using R, a free, flexible and dynamically changing software tool for data analysis that is becoming increasingly popular across the social and behavioural sciences throughout the world." ,"amazon_rating": 4.5,"release_date": "2012-03-22","tags": ["Programming Languages & Tools", "Psychological Methodology", "R Programming"]}
 {"index":{"_index":"books","_id":"48"}}
 {"title": "R for Data Science", "author": "Garrett Grolemund and Hadley Wickham","edition": 1,"synopsis": "What exactly is data science? With this book, you’ll gain a clear understanding of this discipline for discovering natural laws in the structure of data. Along the way, you’ll learn how to use the versatile R programming language for data analysis." ,"amazon_rating": 4.7,"release_date": "2016-07-25","tags": ["Data Wrangling", "Data Visualization", "Exploratory Data Analysis"]}
 {"index":{"_index":"books","_id":"49"}}
 {"title": "Geocomputation with R", "author": "Robin Lovelace, Jakub Nowosad and Jannes Muenchow","edition": 1,"synopsis": "Geocomputation with R is for people who want to analyze, visualize and model geographic data with open source software. It is based on R, a statistical programming language that has powerful data processing, visualization, and geospatial capabilities." ,"amazon_rating": 5.0,"release_date": "2019-03-21","tags": ["Geocomputation", "R Programming"]}
 {"index":{"_index":"books","_id":"50"}}
 {"title": "R Cookbook", "author": "Jd Long and Paul Teetor","edition": 1,"synopsis": "Perform data analysis with R quickly and efficiently with more than 275 practical recipes in this expanded second edition. The R language provides everything you need to do statistical work, but its structure can be difficult to master. These task-oriented recipes make you productive with R immediately. Solutions range from basic tasks to input and output, general statistics, graphics, and linear regression." ,"amazon_rating": 4.6,"release_date": "2019-07-12","tags": ["Data Analysis, Statistics, and Graphics", "R Programming"]}*/