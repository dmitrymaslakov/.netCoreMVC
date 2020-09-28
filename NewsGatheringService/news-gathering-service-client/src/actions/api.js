import axios from "axios";

const baseUrl = 'https://localhost:5001/api/'

export default {

    news(url = baseUrl) {
        return {
            fetchAll: () => axios.get(url+'news/'),
            /*fetchById: id => axios.get(url + id),
            create: newRecord => axios.post(url, newRecord),
            update: (id, updateRecord) => axios.put(url + id, updateRecord),
            delete: id => axios.delete(url + id)*/
        }
    }
}