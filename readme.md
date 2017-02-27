#OUR PROTOTYPE
Trinity Technology Group's application prototype (code name Thunderstruck) will allow California residents to receive email, SMS and push notificaitons relating to emergency and non-emergencies from a variet managed sources.  This application is geo-location aware provides administrators to publish, track, analyze, and visual related data. [Full Requirements](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/Prototype%20B%20Requirements.pdf)
##As a Resident, [link]
##As an administrator, 
##etc [tbd]

#TECHNICAL APPROACH

##A. Assigned one (1) leader and gave that person authority and responsibility and held that person accountable for the quality of the prototype submitted
Jeremy Dean, Director of Business Developent
##B. Assembled a multidisciplinary and collaborative team that includes, at a minimum, five (5) of  the  labor  categories  as  identified  in  Attachment  B:  PQVP DS-AD  Labor  Category  Descriptions  
Our collaborative multidisciplinary team experiece includes over 250,000 hours delivering technical solutions.  
####Product Manager: Jeremy Dean
####Deliver Manager: Michael Tomlin
####Technical Architect: Shawn Sampo 
####Interaction Designer/User Researchers: Kelly Phan, Camille Dyer
####Visual Designer: Kelly Phan
####Front End Developer: Sam Ellis
####Back End Developer: Shawn Sampo
####Dev Ops Engineer: Sam Ellis
####Security Engineer: [Remove?]
####Quality Assurance: Camille Dyer
####Agile Coach: Hiren Vashi, Agile related certifications Inculde:   
Scaled Agile Framework (SAFe®) Program Consultant 4.0 (SPC4)  
Certified Scrum Professional (CSP)  
Certified Scrum Master (CSM)  
PMI-Agile Certified Practitioner (PMI-ACP)  
Project Management Professional (PMP)   
###c.   Understood what people needed, by including people in the prototype development and design process;  
###d.   Used at least a minimum of three (3) “user-centric design” techniques and/or tools; 
####1. Following the USDS Playbook
We used the playbook published by the USDS.  We integrated the playbook into our trello boards for checklists/key questions to facilitate team access. [Intitial Playbook](https://trello.com/b/XZ7T6XhN/digital-playbook), [Work in Progress](https://cloud.githubusercontent.com/assets/23264395/23351792/20e96844-fc78-11e6-87cd-a7e92e07773d.png), [Detailed Checklist Screen](https://cloud.githubusercontent.com/assets/23264395/23351810/46e243e0-fc78-11e6-99b8-b8e69242fefe.png)  
####2. We developed a strong vision statement followed by identifying personas, [Detailed Here](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/UserPersonas2.0.pdf)  
####3. We used tree.taiga.io to manager our user stories, sprints and feedback at the core of our agile development process [Screen Image](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/screen%20images/Tree.taiga.io-user-stories-sprint-1.png)
###e.   Used GitHub to document code commits; 
We used GitHub to document our code commits. An example of the commits can be found here: [GitHub Screen Shot](https://cloud.githubusercontent.com/assets/23264395/23351069/ae61853a-fc73-11e6-9e9a-630ae6d2407b.png)  
###f. Used Swagger to document the RESTful API, and provided a link to the Swagger API; 
###g. Complied with Section 508 of the Americans with Disabilities Act and WCAG 2.0; 
###h. Created or used a design style guide and/or a pattern library;  
###i. Performed usability tests with people; 
Our residents test users performed usability testing. In addition to the our users outside the project team, we also solicited voluteers from outside the team to perform additional user testing.  Our focus was to identify 'real' users that we associated with our personas to make sure we could trace user stories through to completion.
###j.    Used an iterative approach, where feedback informed subsequent work or versions of the prototype;  
The iterative development approach for this prototype uses the Agile/Scrum methodology guilded by our resident Agile Coach (Hiren)  
1. Prior to beging the agile/scrum process, the team setup the facilities including the collaborative work-area, gathered supplies, selected the tools, configured the architecture, and identified each sprint goal  
2. Set up team collaboration site in tree.taiga.io [Evidence?] with the product backlog used to guide team  
3. At the beginning of each sprint, we identified the sprint backlog, held a sprint planning meeting using Agile Planning Poker  
4. Held daily scrum meetings at the same time and place with visibility of the Kanban task board  
5. Team co-location with visible persona's with clearly identified roles and goals  
6. Sprint Demo for review and to gather feedback from Product Owner and proxy users based on personas  
7. Sprint Retrospectives to help course correct the process  
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
Automated unit tests are developed to validate the return types and verify that the controllers return what is expected. An example of unity test [outputs link](https://cloud.githubusercontent.com/assets/23264395/23348944/a984b308-fc63-11e6-880f-4692cd0fd90a.png).    
An example of our embedded [unit test for sprint 1 link](https://cloud.githubusercontent.com/assets/23264395/23374564/af400470-fcd9-11e6-89b6-4aa6e9795a0f.png)  
###o.   Setup  or  used  a  continuous  integration  system  to  automate  the  running  of  tests  and  continuously deployed their code to their IaaS or PaaS provider; 
###p.   Setup or used configuration management; 
###q.   Setup or used continuous monitoring;  
https://papertrailapp.com/  
Jenkins /nagios?  
https://app.google.stackdriver.com/  


###r.    Deployed  their  software  in  an  open  source  container,  such  as  Docker (i.e., utilized  operating-system-level virtualization);  
###s.   Provided sufficient documentation to install and run their prototype on another machine; and 
###t.   Prototype  and  underlying  platforms  used  to  create  and  run  the  prototype  are  openly  licensed and free of charge.
