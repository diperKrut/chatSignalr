export class TokenLocal{

    static SetToken(token){
        localStorage.setItem("Token",token);
    }
    static getToken(){
        return localStorage.getItem("Token");
    }

    static SetRefreshToken(token){
        localStorage.setItem("RefreshToken",token);
    }
    static getRefreshToken(){
        return localStorage.getItem("RefreshToken");
    }
    static getIsAuth(){
        return localStorage.getItem("IsAuth");
    }
    static setIsAuth(bool){
        localStorage.setItem("IsAuth", bool);
    }
}
