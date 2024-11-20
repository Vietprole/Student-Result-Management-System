import API_BASE_URL from "./base-url";
import axios from 'axios';

const API_GIANGVIEN = `${API_BASE_URL}/api/giangvien`;

export const getAllGiangViens = async () => {
  try {
    const response = await axios.get(API_GIANGVIEN);
    return response.data;
  } catch (error) {
    console.log("error message: ", error.message);
  }
};