export const apiUrl={
    apiUrl : window.location.origin === "http://localhost:4200" ? `http://localhost:7614/api` : `${window.location.origin}/api`,
     get getStatusUrl(){
        return `${apiUrl.apiUrl}/status`
     },
     get makePostUrl(){
      return `${apiUrl.apiUrl}/post`
     },
     get passwordsUrl(){
      return `${apiUrl.apiUrl}/passwords`;
     }
     
};
