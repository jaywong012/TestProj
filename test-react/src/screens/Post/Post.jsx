import React, { useEffect } from "react";
import ProductList from "./components/PostList/PostList";
import { useDispatch, useSelector } from "react-redux";
import { Container } from "react-bootstrap";
import { defaultPageIndex, pageSize } from "../../constants/common";
import postApiServices from "@/features/apis/posts/posts";
import {
  setLoading,
  setPages,
  setPosts,
} from "@/features/redux/slicers/postSlice";
import AddEditPost from "./components/AddEditPost";
import ToastMessage from "@/components/ToastMessage/ToastMessage";
import socialAccessInfoApiServices from "@/features/apis/socialAccessInfo/socialAccessInfo";
import { setUserDetail } from "@/features/redux/slicers/socialAccessInfoSlice";
import { socialName } from "@/constants/socialConstant";
import XAuthorize from "../Social/XAuthorize";
import FacebookAuthorize from "../Social/FacebookAuthorize";

const Post = () => {
  const dispatch = useDispatch();
  const currentPage = useSelector((state) => state.post.currentPage);
  const searchKey = useSelector((state) => state.post.searchKey);
  const loading = useSelector((state) => state.post.loading);

  useEffect(() => {
    getPosts();
    getUserSocialAccessInfo();
  }, []);

  const getPosts = async () => {
    try {
      const pageIndex = currentPage ?? defaultPageIndex;
      const searchRequest = {
        searchKey: searchKey,
        pageIndex: pageIndex,
        pageSize: pageSize,
      };
      const result = await postApiServices.getPostsByPaging(searchRequest);
      dispatch(setPosts(result?.posts));
      dispatch(setPages(result?.totalPages));
    } catch (error) {
      console.error("Error fetching products:", error);
    } finally {
      dispatch(setLoading(false));
    }
  };

  const getUserSocialAccessInfo = async () => {
    try {
      const result = await socialAccessInfoApiServices.get();
      const userDetails = result.reduce((acc, item) => {
        const platform = item.type.toLowerCase();
        if (platform === socialName.X || platform === socialName.FACEBOOK) {
          acc[platform] = item;
        }
        return acc;
      }, {});
      dispatch(
        setUserDetail({
          platform: socialName.X,
          data: userDetails[socialName.X],
        })
      );
      dispatch(
        setUserDetail({
          platform: socialName.FACEBOOK,
          data: userDetails[socialName.FACEBOOK],
        })
      );
    } catch (error) {
      console.error("Error fetching products:", error);
    } finally {
      dispatch(setLoading(false));
    }
  };

  const handleDelete = async (id) => {
    await postApiServices.deletePost(id);
    await getPosts();
  };

  return (
    <Container fluid>
      <Container>
        <ToastMessage />
        <AddEditPost getPosts={getPosts} />
        <XAuthorize
          getUserSocialAccessInfo={getUserSocialAccessInfo}
          getPosts={getPosts}
        />
        <FacebookAuthorize
          getUserSocialAccessInfo={getUserSocialAccessInfo}
          getPosts={getPosts}
        />
        <ProductList
          handleDelete={handleDelete}
          loading={loading}
          getPosts={getPosts}
        />
      </Container>
    </Container>
  );
};

export default Post;
