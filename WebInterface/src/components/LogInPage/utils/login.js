
export default async function login(userName, password){
     var user = {
         userName: userName,
         password: password
     };

     let responce = await fetch("https://localhost:44345/api/auth/login", {
         method: "POST",
         headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
         },

         body: JSON.stringify(user)

     });

     let result = responce.json();

     return result;
}