#Assumptions  

We assumed that only the service will need to be put into Docker because of load balancing  
We determined that it would be redundant to put a database in the container  

When users create a profile, the system automatically opts in for every notification type  

Username and password can allow any type of char  

System will not confirm address entered by user  

System will track any recent notifications  

User can choose any role because there is not authentication process (yet)  

Users will want to be able to track more than one location (more than one address)  
User Interface cannot support this now (not yet)  
Database can support this now  
