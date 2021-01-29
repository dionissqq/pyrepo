const loginFormEl = document.getElementById('login-form');
loginFormEl.addEventListener('submit', function (e) {
   console.log("On form submit");
   e.preventDefault();  // cancel page reload
   const formData = new FormData(e.target);
   const bodyData = new URLSearchParams(formData);
   fetch("/auth/login", { method: 'POST', body: bodyData })
       .then(x => x.json())
       .then(authResult => {
           console.log(authResult);
           if (authResult.message=="Login failed"){
                const nadpis = document.getElementById('nadpis');
                nadpis.innerText="no such user"
           }
           else{
                const jwt = authResult.token;
                localStorage.setItem("jwt", jwt);  // save JWT
                window.location.href = "/";
           }
       })
       .catch(console.error);
});

const logoutForm = document.getElementById("logout-form");
logoutForm.addEventListener('submit', function (e) {
   console.log("On logoutForm submit");
   e.preventDefault();  // cancel page reload
   localStorage.removeItem('jwt');
});
