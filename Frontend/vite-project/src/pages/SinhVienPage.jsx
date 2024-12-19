import Layout from "./Layout";
import DataTable from "@/components/DataTable";
import { ArrowUpDown, MoreHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  getSinhViens,
  deleteSinhVien,
} from "@/api/api-sinhvien";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { SinhVienForm } from "@/components/SinhVienForm";
import { getAllKhoas } from "@/api/api-khoa";
import { ComboBox } from "@/components/ComboBox";
import { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { getAllLopHocPhans } from "@/api/api-lophocphan";
import { createSearchURL } from "@/utils/string";

const createSinhVienColumns = (handleEdit, handleDelete) => [
  {
    accessorKey: "id",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Id
          <ArrowUpDown />
        </Button>
      );
    },
    cell: ({ row }) => <div className="px-4 py-2">{row.getValue("id")}</div>,
  },
  {
    accessorKey: "ten",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Tên
          <ArrowUpDown />
        </Button>
      );
    },
    cell: ({ row }) => <div className="px-4 py-2">{row.getValue("ten")}</div>,
  },
  // {
  //   accessorKey: "khoaId",
  //   header: ({ column }) => {
  //     return (
  //       <Button
  //         variant="ghost"
  //         onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
  //       >
  //         Khoa Id
  //         <ArrowUpDown />
  //       </Button>
  //     );
  //   },
  //   cell: ({ row }) => <div className="px-4 py-2">{row.getValue("khoaId")}</div>,
  // },
  {
    accessorKey: "tenKhoa",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Tên Khoa
          <ArrowUpDown />
        </Button>
      );
    },
    cell: ({ row }) => <div className="px-4 py-2">{row.getValue("tenKhoa")}</div>,
  },
  {
    accessorKey: "namBatDau",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Năm Bắt Đầu
          <ArrowUpDown />
        </Button>
      );
    },
    cell: ({ row }) => <div className="px-4 py-2">{row.getValue("namBatDau")}</div>,
  },
  {
    id: "actions",
    enableHiding: false,
    cell: ({ row }) => {
      const student = row.original;

      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <Dialog>
              <DialogTrigger asChild>
                <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                  Sửa Sinh Viên
                </DropdownMenuItem>
              </DialogTrigger>
              <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                  <DialogTitle>Edit Sinh Vien</DialogTitle>
                  <DialogDescription>
                    Edit the current student.
                  </DialogDescription>
                </DialogHeader>
                <SinhVienForm sinhVien={student} handleEdit={handleEdit} />
              </DialogContent>
            </Dialog>
            <Dialog>
              <DialogTrigger asChild>
                <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                  Xóa Sinh Viên
                </DropdownMenuItem>
              </DialogTrigger>
              <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                  <DialogTitle>Delete Sinh Vien</DialogTitle>
                  <DialogDescription>
                    Delete the current student.
                  </DialogDescription>
                </DialogHeader>
                <p>Are you sure you want to delete this Sinh Vien?</p>
                <DialogFooter>
                  <Button
                    type="submit"
                    onClick={() => handleDelete(student.id)}
                  >
                    Delete
                  </Button>
                </DialogFooter>
              </DialogContent>
            </Dialog>
          </DropdownMenuContent>
        </DropdownMenu>
      );
    },
  },
];

export default function SinhVienPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const lopHocPhanIdParam = searchParams.get("lopHocPhanId");
  const khoaIdParam = searchParams.get("khoaId");
  const [data, setData] = useState([]);
  const [khoaItems, setKhoaItems] = useState([]);
  const [lopHocPhanItems, setLopHocPhanItems] = useState([]);
  const [khoaId, setKhoaId] = useState(khoaIdParam);
  const [lopHocPhanId, setLopHocPhanId] = useState(lopHocPhanIdParam);
  const [comboBoxKhoaId, setComboBoxKhoaId] = useState(khoaIdParam);
  const [comboBoxLopHocPhanId, setComboBoxLopHocPhanId] = useState(lopHocPhanIdParam);
  const baseUrl = "/sinhvien";

  useEffect(() => {
    const fetchData = async () => {
      const dataKhoa = await getAllKhoas();
      const mappedKhoaItems = dataKhoa.map(khoa => ({ label: khoa.ten, value: khoa.id }));
      setKhoaItems(mappedKhoaItems);

      const dataLopHocPhan = await getAllLopHocPhans();
      const mappedLopHocPhanItems = dataLopHocPhan.map(lhp => ({ label: lhp.ten, value: lhp.id }));
      setLopHocPhanItems(mappedLopHocPhanItems);

      const data = await getSinhViens(khoaId, lopHocPhanId);
      setData(data);
    };
    fetchData();
  }, [khoaId, lopHocPhanId]);

  const handleGoClick = () => {
    setKhoaId(comboBoxKhoaId);
    setLopHocPhanId(comboBoxLopHocPhanId);
    const url = createSearchURL(baseUrl, { 
      khoaId: comboBoxKhoaId, 
      lopHocPhanId: comboBoxLopHocPhanId 
    });
    navigate(url);
  };

  return (
    <Layout>
      <div className="w-full">
        <div className="flex">
          <ComboBox items={khoaItems} setItemId={setComboBoxKhoaId} initialItemId={khoaId}/>
          <ComboBox items={lopHocPhanItems} setItemId={setComboBoxLopHocPhanId} initialItemId={lopHocPhanId}/>
          <Button onClick={handleGoClick}>Go</Button>
        </div>
        <DataTable
          entity="Sinh Vien"
          createColumns={createSinhVienColumns}
          data={data}
          setData={setData}
          deleteItem={deleteSinhVien}
          columnToBeFiltered={"ten"}
          ItemForm={SinhVienForm}
        />
      </div>
    </Layout>
  );
}
