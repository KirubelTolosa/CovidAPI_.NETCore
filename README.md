# COVID-19 REST API
This covid-19 Rest-API can be used to retrieve global covid related data, ConfirmedCases, Deaths, and Recoveries. It contains an injestion console application which ports over .csv format data from the source to store in a Microsoft SQL Server database, and WebApplication that exposes several endpoints to query the database. </br>

### The technologies/libraries used in the application are the following
<ul>
    <li>Autofac (For dependency injection)</li>    
    <li>ASP.NET (For the web application)</li>
    <li>ADO.NET (For the data access to SQL db)</li>
    <li>CSVHelper (For reading .csv responce)</li>
    <li>AWS Toolkit for VS(Deployment to elastic beanstalk)</li>
    <li>SQL Server Replication (To migrate local DB to Amazon RDS)</li>
    <li>....</li>        
</ul>


Hosting the application requires changing the endpoint to the databases and scheduling a task to update it with recent data. The running version of the application is running in aws elasticbeanstalk environment at the specified urls below. A scheduled task is running every five minutes on a t2.micro EC2 instance to make a call to data source API, filter the most recent data and replenish the database.</br>

*** The data source I used is provided by <i>"Johns Hopkins University_Center for Systems Science and Engineering (CSSE).</i>

### URI to access running versions of the application:
(Documentation included)
    <div>
      www.kirubeltolosa.com
    </div>
  <h3>Here are a few example URI refernces to resource usage</h3>  
  <br>
    <pre>
    
    Use the following uri reference to get national count of cases(metrics) from all nations in the world.  
              "kirubeltolosa.com/api/covid/Confirmed_Cases",
              "kirubeltolosa.com/api/covid/Deaths",
              "kirubeltolosa.com/api/covid/Recoveries"                  
              
    Use the following uri reference to get the worldwide count of cases(metrics). 
             "kirubeltolosa.com/api/covid/{metrics}/GlobalCount"
                    
    Use the following uri reference to get the national count of cases(metrics) of a nation. Optionally, you can include a date to find the count of cases on that date. 
            "kirubeltolosa.com/api/covid/{metrics}/Country/{Date : Optional}"
           
    Use the following uri refernce to get the totalled count of cases of nation for each tracked date. 
            "kirubeltolosa.com/api/covid/{metrics}/Country/DailyCount"  
   </pre>
</br> 
 
<i>I hope you will find this useful and please reach out to me at info@kirubeltolosa.com if you have any question or feedback. Thanks. </i>
