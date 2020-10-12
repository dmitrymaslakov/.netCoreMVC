import { userConstants } from '../constants/user.constants';
import { urlConstants } from '../constants/url.constants';


export const getProfileFetch = () => {
    let payload;

    /*const token = localStorage.token;
    if (token) {
        return fetch(urlConstants.USER_AUTHENTICATE, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                Accept: 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
            .then(resp => resp.json())
            .then(data => {
                if (data.message) {
                    // Будет ошибка если token не дествительный
                    localStorage.removeItem("token")
                } else {
                    payload = data.userName;
                }
            })
    }*/
    return {
        type: userConstants.USER_LOGIN,
        payload: payload
      }
    

}
