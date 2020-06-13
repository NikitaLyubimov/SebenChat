let url = "https://localhost:44345/api/auth/register";

export default async function registerPost(firstName, secondName, userName,email, password)
{
    let user = {
        firstName: firstName,
        secondName: secondName,
        userName: userName,
        email: email,
        password: password
    };
    console.log(JSON.stringify(user));
    let responce = await fetch("https://localhost:44345/api/auth/register", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
          
        body: JSON.stringify(user)
    });

    let result = await responce.json();

    return result; 


}