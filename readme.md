####This readme.md file is presented to meet the requirement of communicating our project approach including the technical approach within a 2,000 character limit.
#OUR RESULTS
###[Read the Quick Start Guide](TBD) The application is a hosted application that does not require any locally installed components.  
###[Run the Prototype](http://avpwebappui.azurewebsites.net/thunderstruck.html) (code name Thunderstruck).  This application will allow California residents to receive emergency and non-emergency emails, SMS, and push notifications from several identified [sources](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/RFI_CDT-ADPQ-0117_-_Prototype_B_Resources.pdf).  This application is geo-location aware, provides administrators to ability publish, track, analyze, and visualize related data. [Full Requirements](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/Prototype%20B%20Requirements.pdf)
[CDT GitHub Site](https://github.com/CDTProcurement/adpq)

#OUR PROJECT APPROACH
###The project approach was driven by a very compressed schedule with two distinct tracks of requirements  
1. Responding to the Response for Inquiry (RFI) Requirement's administrative/Submission requirements  
2. Building working software using an Agile/iterative approach based on the [Digital Services Playbook](https://playbook.cio.gov/).

##Responding to the RFI
####The team used TrinityTG's proven approach to manage the RFI response by tracking the state procurement site and monitoring the procurement GitHub site while breaking down the RFI so we could build-up our response. With our proposal team focusing on the RFI response, our Director of Business Development was able to lead the resolution of requirement #2 as the Product Manager (play 6). Responding to the RFI is not documented beyond this brief description.  
##Building Working Software
####We also turned to our proven approach to managing the software build. This included assembling our highly-qualified team and building our project playbook. Our certified Agile Coach worked with the Product Manager (aka Product Owner) and the Delivery Manager (aka Scrum Master) to layout an Agile based project playbook that included a dedicated working area in our Sacramento office for the team to co-locate and collaborate. This playbook resulted in the software build described in our technical approach below. In this project approach  section, we focus on the details that do not clearly fit within the "a. through t." punch list below.  
####Given the theoretical nature the engagement, we had to make many assumptions that are documented [here](TBD). Our experienced multidisciplinary team (play 7) has over 250,000 hours delivering technical solutions to real people (play 1) in a primarily iterative manner (play 4). Our certified Agile Coach helped raise the agility of the team (play 4) so we could complete the build iteration steps detailed in J. below. Also given the prototype project scope, timeline and staffing, budgets (play 5) and security (play 11) were not a factor in our project.  We understood the emphasis was to quickly move through multiple, very short sprint/iterations to demonstrate our understanding of the process. 
####Given our proxy user base, a particular problem the team had to overcome was how to focus on user centric design (play 1, play 2, and play 3). This difficulty was eased by using personas to simuluate or user community and to serve as the basis of our product manager to develop the user stories. This approach paid dividends when our proxy users studied their personas and 'got into character' for design, testing and training efforts.   
####Given our team's extensive experience with open source tools, when the goal was to "default to open (play 13)", we were able to integrate nearly three times as many open source tools than required by the RFI. 
#Evidence.
####By following the links in this paragraph, the evidence of our team's Agile process is documented within GitHub.  This evidence includes our artifacts from our open source Agile management tool "tree.taiga.io", "Action Shots" of the team during our Agile ceremonies and collaboration sessions, and many "screen prints" of code, smartphone photos of white boards as well as other more traditional documentation. The digital assets necessary to deploy and continue enhnancing this project are also available on GitHub.
#OUR TECHNICAL APPROACH
##A. Assigned one (1) leader and gave that person authority and responsibility and held that person accountable for the quality of the prototype submitted. 
Using the [Digital Services Playbook - Play 6](https://playbook.cio.gov/#play6) as a guide, we identified **Jeremy Dean**, Director of Business Development as our Product Manager.
##B. Assembled a multidisciplinary and collaborative team that includes, at a minimum, five (5) of  the  labor  categories  as  identified  in  Attachment  B:  PQVP DS-AD  Labor  Category  Descriptions  
####Product Manager: Jeremy Dean
####Deliver Manager: Michael Tomlin
####Technical Architect: Shawn Sampo 
####Interaction Designer/User Researchers: Kelly Phan, Camille Dyer
####Visual Designer: Kelly Phan
####Front End Developer: Sam Ellis
####Back End Developer: Shawn Sampo
####Back End Developer (GIS): Charan Misha  
####Dev Ops Engineer: Sam Ellis
####Security Engineer: [Remove?]
####Quality Assurance: Camille Dyer
####Agile Coach: Hiren Vashi, Agile related certifications Include:   
Scaled Agile Framework (SAFe®) Program Consultant 4.0 (SPC4)  
Certified Scrum Professional (CSP)  
Certified Scrum Master (CSM)
PMI-Agile Certified Practitioner (PMI-ACP)  
Project Management Professional (PMP)   
###c.   Understood what people needed, by including people in the prototype development and design process;  
Using the [Digital Services Playbook - Play 1](https://playbook.cio.gov/#play1) as a guide, we worked with available resources as both proxy and 'real' users as we developed user personas, user stories and user tested prototypes, [TBD link our evidence]  
###d.   Used at least a minimum of three (3) “user-centric design” techniques and/or tools; 
####1. Following the USDS Playbook
We used the playbook published by the USDS.  We integrated the playbook into our trello boards for checklists/key questions to facilitate team access. Using the [The Digital Services Playbook](https://playbook.cio.gov/), our team managed the [work in progress](https://cloud.githubusercontent.com/assets/23264395/23351792/20e96844-fc78-11e6-87cd-a7e92e07773d.png), including adding the [detailed checklists](https://cloud.githubusercontent.com/assets/23264395/23351810/46e243e0-fc78-11e6-99b8-b8e69242fefe.png).  
####2. The second user centric design approach included developing [personas](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/UserPersonas2.0.pdf) to help summarizing our target audience for product development. We further emphasised the personas by making sure our persona [portraits where displayed in team development area](https://cloud.githubusercontent.com/assets/23264395/23387351/bdc5b102-fd10-11e6-9753-902d57caca76.jpg).  
####3. We used tree.taiga.io to manager our user stories, sprints and feedback at the core of our agile development process [Screen Image](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/screen%20images/Tree.taiga.io-user-stories-sprint-1.png)
###e.   Used GitHub to document code commits; 
We used GitHub to document our code commits. The GitHub repository is [here](https://github.com/sellisttg/AVP).  An example of the commits can be found here: [GitHub Screen Shot](https://cloud.githubusercontent.com/assets/23264395/23351069/ae61853a-fc73-11e6-9e9a-630ae6d2407b.png)  
###f. Used Swagger to document the RESTful API, and provided a link to the Swagger API; 
Our team used Swagger to document the RESTful API. [Swagger Live Link(http://avp2017webapp.azurewebsites.net/swagger/index.html)](http://avp2017webapp.azurewebsites.net/swagger/index.html), [GitHub Image link](https://cloud.githubusercontent.com/assets/23264395/23522378/9d6ad4cc-ff37-11e6-8ba7-e150f9639be7.png).   
###g. Complied with Section 508 of the Americans with Disabilities Act and WCAG 2.0; 
###h. Created or used a design style guide and/or a pattern library;  
###i. Performed usability tests with people; 
Our residents test users performed usability testing. In addition to the our users outside the project team, we also solicited volunteers from outside the team to perform additional user testing.  Our focus was to identify 'real' users that we associated with our personas to make sure we could trace user stories through to completion.
###j.    Used an iterative approach, where feedback informed subsequent work or versions of the prototype;  
The iterative development approach for this prototype uses the Agile/Scrum methodology guided by our resident Enterprise Agile Coach (Hiren)  
1. Prior to beginning the agile/scrum process, the team set up the facilities including the collaborative work area, gathered supplies, selected the tools, configured the architecture, and identified each sprint goal  
2. Set up team collaboration site in [tree.taiga.io](https://tree.taiga.io/project/hdv-avp) with the product backlog used to guide the development team  
3. Periodically product owner and development team groomed product backlog [(used Planning Poker)](https://cloud.githubusercontent.com/assets/23264395/23377082/6d2ef5be-fce3-11e6-8f10-9d5e17832427.jpg)      
4. At the beginning of each sprint, we identified the sprint backlog during sprint planning meeting  
5. Held daily scrum meetings at the same time and place with visibility of the Kanban task board  
6. Team co-location with visible persona's with clearly identified roles and sprint goals  
7. Sprint Demo for review and to gather feedback from Product Owner and proxy users based on personas  
8. Sprint Retrospectives to help course correct the process  
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
###n.  Developed automated unit tests for their code;
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
####This prototype application is depoyed as a web application that only requires a browswer to access.  No locally installed software is needed. The server based installation aspects are covered under r. above.  The application url is [http://avpwebappui.azurewebsites.net/thunderstruck.html](http://avpwebappui.azurewebsites.net/thunderstruck.html)
###t. Prototype  and  underlying  platforms  used  to  create  and  run  the  prototype  are  openly  licensed and free of charge.
####In addition to each individual open source license associated with item l. above, TrinityTG has also provided an open source license to this prototype solution [here](https://github.com/sellisttg/AVP/blob/master/AVP%20GitHub%20docs/MITLicense.md)
