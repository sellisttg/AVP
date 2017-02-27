#OUR PROTOTYPE
Trinity Technology Group's Thunderstruck is an application prototype allowing California residents to receive email, SMS and push notificaitons relating to emergency and non-emergencies from a variet managed sources.  This application is geo-location aware provides administrators to publish, track, analyze, and visual related data. ({Link?} https://github.com/sellisttg/AVP/AVP GitHub docs/Prototype B Requirements.pdf

##As a Resident, [link]
##As an administrator, I want to Monitor [link]  
##As an administrator, I want to visualize [link]  
##etc [tbd]

#TECHNICAL APPROACH

##A. Assigned one (1) leader and gave that person authority and responsibility and held that person accountable for the quality of the prototype submitted
Jeremy Dean, Director 
##B. Assembled a multidisciplinary and collaborative team that includes, at a minimum, five (5) of  the  labor  categories  as  identified  in  Attachment  B:  PQVP DS-AD  Labor  Category  Descriptions
###Product Manager: Jeremy Dean
###Deliver Manager: Michael Tomlin
###Technical Architect: 
###Interaction Designer/User Researcher: 
###Visual Designer:
###Front End Developer:
###Back End Developer: 
###Dev Ops Engineer: 
###Security Engineer: 
###Agile Coach: Hiren Vashi
###Quality Assurance: 
###c.   Understood what people needed, by including people in the prototype development and design process;  
###d.   Used at least a minimum of three (3) “user-centric design” techniques and/or tools; 
####Following the USDS Playbook  
We used the playbook published by the USDS.  We integrated the playbook into our trello boards for checklists/key questions to facilitate team access.  https://trello.com/b/XZ7T6XhN/digital-playbook
###e.   Used GitHub to document code commits; 
We used GitHub to document our code commits.  [TBD Link of commit logs?]
###f.    Used Swagger to document the RESTful API, and provided a link to the Swagger API; 
###g.   Complied with Section 508 of the Americans with Disabilities Act and WCAG 2.0; 
###h.   Created or used a design style guide and/or a pattern library;  
###i.    Performed usability tests with people; 
Our residents test users performed usability testing. In addition to the our users outside the project team, we also solicited voluteers from outside the team to perform additional user testing.  Our focus was to identify 'real' users that we associated with our personas to make sure we could trace user stories through to completion.
###j.    Used an iterative approach, where feedback informed subsequent work or versions of the prototype;  
###k.   Created a prototype that works on multiple devices, and presents a responsive design;  
###l.    Used at least five (5) modern and open-source technologies, regardless of architectural layer (frontend, backend, etc.);  
List of Open Source Software  
1. .NET Core https://docs.microsoft.com/en-us/dotnet/articles/core/   
2. MySQL https://www.mysql.com/  
3. Angular.js [Need Link]  
4. Bootstrap http://getbootstrap.com/   
5. JQuery https://jquery.com/  
6. Chart.js* https://github.com/chartjs  
7. Report.js* http://bi-joe.github.io/report.js/ [Need to add]  
8. SignalR* https://github.com/SignalR/SignalR [Need to add]  
9. JWT (we conform to an open standard, but used no library to implement this)     
10. MySqlConnector https://github.com/mysql-net/MySqlConnector  
11. tree.tiaga.io - project management - http://tree.taiga.io  
12. xUnit for unit testing. 

###m.  Deployed the prototype on an Infrastructure as a Service (IaaS) or Platform as Service (PaaS) provider, and indicated which provider they used; 
###n.   Developed automated unit tests for their code;
Automated unit tests are developed to validate the return types and verify that the controllers return what is expected. An example of unity test outputs can be found here (https://cloud.githubusercontent.com/assets/23264395/23348944/a984b308-fc63-11e6-880f-4692cd0fd90a.png "View".  An example of our embedded tests can be viewed here: [NEED CODE]
###o.   Setup  or  used  a  continuous  integration  system  to  automate  the  running  of  tests  and  continuously deployed their code to their IaaS or PaaS provider; 
###p.   Setup or used configuration management; 
###q.   Setup or used continuous monitoring;  
https://papertrailapp.com/  
Jenkins /nagios?  
https://app.google.stackdriver.com/  


###r.    Deployed  their  software  in  an  open  source  container,  such  as  Docker (i.e., utilized  operating-system-level virtualization);  
###s.   Provided sufficient documentation to install and run their prototype on another machine; and 
###t.    Prototype  and  underlying  platforms  used  to  create  and  run  the  prototype  are  openly  licensed and free of charge.
