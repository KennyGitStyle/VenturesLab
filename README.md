# VenturesLab Tech Test Instruction Guide
### Navigating to the VenturesLab_Test Directory
First, let's get you set up in the VenturesLab_Test directory. I assume you've already cloned or downloaded this directory to your computer. On my machine, it’s located in C:\Projects\VenturesLab_Test. Let me walk you through how to navigate there:

Open PowerShell (or Terminal on Mac): Start by opening PowerShell if you're on Windows, or Terminal if you're on a Mac.

Change Directory: In the PowerShell or Terminal window, type cd C:\Projects\VenturesLab_Test and press Enter. This command changes your current working directory to VenturesLab_Test. Remember to replace C:\Projects\VenturesLab_Test with the path where your directory is located on your machine.

Confirm Directory: After pressing Enter, you're now inside the VenturesLab_Test directory. You can type pwd (which stands for "print working directory") to confirm that you're in the correct directory.

List Directory Contents: Type ls to list all the contents in the VenturesLab_Test directory. You should see three projects: API, Infrastructure, and Domain.

Running the API Project
Now, let’s dive into the API project:

Navigate to API Project: Type cd API to enter the API project directory.

Start the API: In the API directory, type dotnet watch run. This command starts the API, and it should be accessible via a local URL, typically http://localhost:5500 (the port number may vary).

### Up and Running with Docker
Install Docker: If you don't have Docker installed, download and install it from the official Docker website.

Pull PostgreSQL Image: Open your terminal and pull the PostgreSQL image from Docker Hub by running the following command:

shell
Copy code
docker pull postgres
Run PostgreSQL Container: Start a PostgreSQL container with the following command. Replace <your_postgres_password> with your desired password and <your_database_name> with your preferred database name:

shell
Copy code
```docker run --name postgres-container -e POSTGRES_PASSWORD=<your_postgres_password> -e POSTGRES_DB=<your_database_name> -p 5432:5432 -d postgres```
This command will create and run a PostgreSQL container with the specified settings.

Pull Redis Image: Pull the Redis image from Docker Hub:

shell
Copy code
docker pull redis
Run Redis Container: Start a Redis container:

shell
Copy code
```docker run --name redis-container -p 6379:6379 -d redis```
This command will create and run a Redis container.

Confirm Containers are Running: To ensure both containers are running, use the following command:

shell
Copy code
docker ps
You should see both the PostgreSQL and Redis containers listed as running.

Sharing Dockerfile:

Create a Dockerfile: To share your Dockerfile, create one in your project directory or an appropriate location. The Dockerfile typically contains instructions to build your application's image.

Copy Dockerfile Contents: Open the Dockerfile and copy its contents.

Share in this Conversation: Paste the Dockerfile contents in this conversation. You can use triple backticks (```) to format it as a code block for better readability.

Save and Build: Save the Dockerfile in your project directory. To build an image from the Dockerfile, navigate to the directory containing the Dockerfile in your terminal and run:

shell
Copy code
docker build -t my-app-image .
Replace my-app-image with a suitable name for your image.

Share Image: Once your image is built, you can share it by pushing it to a container registry (e.g., Docker Hub) or by sharing the image file.

That's it! You've set up PostgreSQL and Redis using Docker and shared your Dockerfile. Others can use your Dockerfile to build the same environment and image for your application.


### Using the API with Swagger UI
The API provides 7 endpoints, including 4 for retrieving data (GET), and others for creating (POST), updating (PUT), and deleting (DELETE) data.

### GET API By sorted upcoming:
Access Swagger UI: Open your web browser and navigate to the Swagger UI at http://localhost:5500/swagger (replace with the correct port number if different).

Sort Data Endpoint: Let's try the /api/usertasks_bysorting endpoint. In Swagger UI, find this endpoint and click on the GET button next to it.

Set Parameters and Execute:

Click the Try it out button.
Choose how you want to sort: by year, month, day, or dayOfWeek.
Set the Ascending parameter to true for ascending order or false for descending order.
Click Execute to send the request and view the response.

### GET Grouping User Tasks by Date
Using the /api/usertasks_grouping Endpoint
The /api/usertasks_grouping endpoint provides a convenient way to view user tasks grouped by their dates in ascending order. This endpoint does not require any additional parameters, making it straightforward to use.

#### How to Access and Use the Endpoint
Open Swagger UI:

Launch your web browser and navigate to Swagger UI, typically found at http://localhost:5500/swagger. Replace 5500 with the actual port number used by your application.
Find the Endpoint:

Scroll through the available endpoints in Swagger UI to locate /api/usertasks_grouping.
Execute the Request:

Click on the GET button next to the /api/usertasks_grouping endpoint.
Then, click Try it out followed by the Execute button. Since this endpoint doesn't require any parameters, you can execute the request directly.
Understanding the Response
After executing the request, you'll receive a response that includes a list of user tasks.
These tasks are automatically grouped by their dates, presented in ascending order.
Each group corresponds to a unique date, under which all tasks scheduled for that date are listed.
Example Scenario
For example, if you want an overview of upcoming tasks organized by date, the /api/usertasks_grouping endpoint does just that. With a simple GET request, you'll get a structured list showing what tasks are planned for each upcoming day, starting from the earliest date.

### Retrieving a Specific User Task by ID
Using the /api/usertask/{id} Endpoint
The /api/usertask/{id} endpoint allows you to retrieve detailed information about a specific user task by providing its unique identifier (ID). The ID is a Guid (Globally Unique Identifier).

How to Access and Use the Endpoint
Open Swagger UI:

Start by opening your web browser and navigating to Swagger UI, usually located at http://localhost:5500/swagger. Remember to replace 5500 with the actual port number if it differs.
Locate the Endpoint:

In Swagger UI, find the /api/usertask/{id} endpoint. This endpoint is designed to retrieve information about a single user task based on its Guid.
Provide the Task ID:

Click on the GET button next to the /api/usertask/{id} endpoint.
Click Try it out to enable input fields.
In the id field, enter the Guid of the user task you wish to retrieve. The Guid should be in the standard format (e.g., 123e4567-e89b-12d3-a456-426614174000) here is an idea you can use "0014c511-a9c1-4ea3-8ddc-d5142133fda9".
Execute the Request:

Click Execute to send the request. Swagger UI will display the request URL, the curl command used, and the response body and status code.
Understanding the Response
The response will include detailed information about the user task associated with the provided Guid.
If a task with the specified ID exists, you'll see its details, such as the date, time, subject, description, etc.
If no task is found with that ID, you may receive an error message or an empty response, depending on how the API is designed.

### Retrieving a List of Items
Using the /api/usertasks Endpoint
The /api/usertasks endpoint is designed to provide a list of all user tasks available in the system. This is a straightforward and easy-to-use endpoint that does not require any parameters.

How to Access and Use the Endpoint
Open Swagger UI:

Begin by launching your web browser and navigating to Swagger UI, typically found at http://localhost:5500/swagger. Adjust the port number (5500) if your setup uses a different one.
Find the Endpoint:

Scroll through Swagger UI to locate the /api/usertasks endpoint.
Execute the Request:

Click on the GET button next to the /api/usertasks endpoint.
Then, click Try it out followed by the Execute button. Since this endpoint does not require any specific parameters, you can send the request right away.
Understanding the Response
The response from this endpoint will include a list or an array of user tasks.
Each item in the list will contain details about a specific user task, such as its ID, date, subject, description, and any other relevant information.
This endpoint is particularly useful for getting an overview of all tasks or for initial loading of task data in client applications.



### Creating a New User Task
Using the POST /api/usertask Endpoint
The POST /api/usertask endpoint allows for the creation of new user tasks. This endpoint expects a JSON payload containing the details of the task to be created.

How to Access and Use the Endpoint
Open Swagger UI:

Start by opening a web browser and navigating to Swagger UI at http://localhost:5220/swagger. Ensure the port number (5220) matches the one used by your application.
Locate the Endpoint:

Find the POST /api/usertask endpoint in the list of available endpoints on Swagger UI.
Input Task Details:

Click on the POST button next to the /api/usertask endpoint.
Click Try it out to enable the input fields.
You'll need to input the task details in the request body. This typically includes fields like UserId, CurrentDate, StartTime, EndTime, Subject, Description, and IsCurrentDate. The exact fields depend on your API's requirements.
**Real JSON payload for Testing**:
```
{
    "id": "1a424eaa-26a7-4f88-b599-bb8e21b6e5a9",
    "userId": "c218d16d-ae7c-445e-87f3-78573f524bee",
    "currentDate": "2023-12-01",
    "startTime": "06:00:00",
    "endTime": "09:00:00",
    "subject": "Meeting with Manager",
    "description": "Manager meeting to discuss project progress",
    "isCurrentDate": true
}

```
**Execute the Request**:

After filling in the task details, click Execute to send the request.
Understanding the Response
Upon successful creation, the API might return a success message, the details of the created task, or simply a status code indicating success (e.g., HTTP 201 Created).
If there's an issue with the task data (like missing required fields), the API might return an error response detailing what needs to be corrected.


### Updating an Existing User Task
Using the PUT /api/usertask/{id} Endpoint
The PUT /api/usertask/{id} endpoint is designed to update a specific user task. You'll need to provide the unique identifier (ID) of the task you want to update and a JSON payload with the updated task details.

How to Access and Use the Endpoint
Open Swagger UI:

Launch your web browser and navigate to Swagger UI at http://localhost:5220/swagger. Ensure you're using the correct port number.
Find the Endpoint:

Scroll to locate the PUT /api/usertask/{id} endpoint in Swagger UI.
Provide Task ID and Updated Details:

Click on the PUT button next to the /api/usertask/{id} endpoint.
Click Try it out to enable input fields.
In the id field, enter the Guid of the user task you wish to update.
In the request body field, input the updated details of the task. This might include fields like CurrentDate, StartTime, EndTime, Subject, Description, and IsCurrentDate.
**Real JSON payload to change the currentDate**:
```
{
    "id": "6dc4bc7c-d1a9-4908-96b2-28054161fb93",
    "userId": "c218d16d-ae7c-445e-87f3-78573f524bee",
    "currentDate": "2024-12-02",
    "startTime": "11:00:00",
    "endTime": "15:00:00",
    "subject": "Manager Call",
    "description": "Call with manager to discuss requirements",
    "isCurrentDate": false
}
```

**Execute the Request**:

After entering the task ID and updating the details, click Execute to send the request.
Understanding the Response
If the update is successful, the API might return a success message, the details of the updated task, or a status code like HTTP 204 No Content.
If the task with the specified ID does not exist or if there is a problem with the provided data (such as validation errors), the API will return an appropriate error response.


### Deleting a User Task
Using the DELETE /api/usertask/{id} Endpoint
The DELETE /api/usertask/{id} endpoint is used to remove a specific user task from the system. This action requires the unique identifier (ID) of the task to be deleted.

How to Access and Use the Endpoint
Open Swagger UI:

Begin by opening a web browser and navigating to Swagger UI, typically located at http://localhost:5220/swagger. Make sure the port number is correct for your application.
Locate the Endpoint:

In Swagger UI, find the DELETE /api/usertask/{id} endpoint.
**Specified Task ID for Testing**: 1a424eaa-26a7-4f88-b599-bb8e21b6e5a9

Click on the DELETE button next to the /api/usertask/{id} endpoint.
Click Try it out to enable the input field for the ID.
Enter the Guid of the user task you want to delete in the id field.
Execute the Request:

Click Execute to send the deletion request. This will instruct the server to remove the task associated with the provided ID.
Understanding the Response
Upon successful deletion, the API might return a success status code like HTTP 200 OK or HTTP 204 No Content.
If no task with the provided ID exists, or if there's an issue processing the request, the API will return an appropriate error response, such as HTTP 404 Not Found.