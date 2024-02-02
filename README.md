# VOCALI-Challenge

Hi code reviewer. Here we have the project which tries to solve the challenge.

## Architecture design
The module is just a console application with has two main classes `TranscriptProcessor` and `InvoxService`.
About the requirement of running it at 00:00 everyday, I didn't create a logic for that but leave the console application ready to be invoked from any place. Inserting this logic in the application could cause less flexibility to run this application in any other scenario (such as reprocess at other time). For this time requirement I though in a external job which calls to our application.

## Code
The code was organized for avoiding overdesign it. 
The path where from we have to take the audio files can be passed as command line paramenter so we can take files from different sources.
On `TranscriptProcessor` class we have the main logic of retries, batch size, calling external service, storing the txt file and so on. 
`InvoxService` is our mock. On it we have 5% percentege errors and retrieving transcript result from a file.
Also we have several support classes such as the helpers that helps us to separate the stuffs as better as possible.

## Unit testing
At unit teting I tried to cover the most logic I could. Validation like file size, retries logic, save file, between other were covered as well. 
