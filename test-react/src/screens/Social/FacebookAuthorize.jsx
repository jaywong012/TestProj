import React from "react";
import { auth } from "@/config/firebaseConfig";
import { FacebookAuthProvider, signInWithPopup } from "firebase/auth";
import { useDispatch, useSelector } from "react-redux";
import { setUserDetail } from "@/features/redux/slicers/socialAccessInfoSlice.js";
import { Button, Container } from "react-bootstrap";
import socialAccessInfoApiServices from "@/features/apis/socialAccessInfo/socialAccessInfo";
import { socialName } from "@/constants/socialConstant";

const FacebookAuthorize = ({getPosts, getUserSocialAccessInfo}) => {
  const fbProvider = new FacebookAuthProvider();
  const dispatch = useDispatch();
  const fbUser = useSelector((state) => state.socialAccessInfo.userDetails.facebook);

  const handleSignIn = async () => {
    try {
      const result = await signInWithPopup(auth, fbProvider);
      const userDetail = result._tokenResponse;
      const user = {
        userName: userDetail.displayName,
      };
      dispatch(setUserDetail({ platform: socialName.FACEBOOK, data: user }));
      const authRequest= {
        accessToken: userDetail.oauthAccessToken,
        type: socialName.FACEBOOK,
        userName: userDetail.displayName
      }
      socialAccessInfoApiServices.create(authRequest);
      getPosts();
    } catch (error) {
      console.error("FB Sign-In Error", error);
    }
  };

  const handleSignOut = async () => {
    await socialAccessInfoApiServices.delete(fbUser.id);
    getUserSocialAccessInfo();
  }

  return (
    <Container>
      {fbUser?.userName ? (
        <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
          <p style={{marginBottom: 0}}>FB:</p>
          <p style={{marginBottom: 0}}>Welcome, {fbUser.userName}</p>
          <Button variant="danger" onClick={handleSignOut}>X</Button>
        </div>
      ) : (
        <Button onClick={handleSignIn}>Sign in with FB</Button>
      )}
    </Container>
  );
};

export default FacebookAuthorize;
