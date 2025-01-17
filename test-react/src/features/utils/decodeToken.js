const decodeJwtToken = () => {
    const token = localStorage.getItem("token");
    const userDetail = jwtDecode(token);
    return userDetail;
}

export default decodeJwtToken;