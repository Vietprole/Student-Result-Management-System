import Layout from "./Layout";
import DataTable from "@/components/DataTable";
import { ArrowUpDown, MoreHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";
import { getAllHocPhans, deleteHocPhan } from "@/api/api-hocphan";
import { getNganhs } from "@/api/api-nganh";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  DialogClose,
} from "@/components/ui/dialog";
import { HocPhanForm } from "@/components/HocPhanForm";
import { useRef, useState, useEffect } from "react";
import AddPLOToHocPhanForm from "@/components/AddPLOToHocPhanForm";
import ManagePLOInHocPhanForm from "@/components/ManagePLOInHocPhanForm";
import { ComboBox } from "@/components/ComboBox";

export default function HocPhanPage() {
  const addPLOFormRef = useRef(null);
  const managePLOFormRef = useRef(null);
  const [data, setData] = useState([]);
  const [nganhItems, setNganhItems] = useState([]);
  const [selectedNganhId, setSelectedNganhId] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      const data = await getAllHocPhans();
      setData(data);
      
      const dataNganh = await getNganhs();
      const mappedNganhItems = dataNganh.map(nganh => ({ label: nganh.ten, value: nganh.id }));
      setNganhItems(mappedNganhItems);
    };
    fetchData();
  }, []);

  const createHocPhanColumns = (handleEdit, handleDelete) => [
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
  {
    accessorKey: "soTinChi",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Số Tín Chỉ
          <ArrowUpDown />
        </Button>
      );
    },
    cell: ({ row }) => (
      <div className="px-4 py-2">{row.getValue("soTinChi")}</div>
    ),
  },
  {
    accessorKey: "laCotLoi",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Là Cốt Lõi
          <ArrowUpDown />
        </Button>
      );
    },
    cell: ({ row }) => (
      <div className="px-4 py-2">{row.getValue("laCotLoi").toString()}</div>
    ),
  },
  {
    accessorKey: "khoaId",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Khoa Id
          <ArrowUpDown />
        </Button>
      );
    },
    cell: ({ row }) => (
      <div className="px-4 py-2">{row.getValue("khoaId")}</div>
    ),
  },
  {
    id: "actions",
    enableHiding: false,
    cell: ({ row }) => {
      const item = row.original;

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
                  Sửa Học Phần
                </DropdownMenuItem>
              </DialogTrigger>
              <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                  <DialogTitle>Edit HocPhan</DialogTitle>
                  <DialogDescription>Edit the current item.</DialogDescription>
                </DialogHeader>
                <HocPhanForm hocphan={item} handleEdit={handleEdit} />
              </DialogContent>
            </Dialog>
            <Dialog>
              <DialogTrigger asChild>
                <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                  Xóa Học Phần
                </DropdownMenuItem>
              </DialogTrigger>
              <DropdownMenuSeparator />
              <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                  <DialogTitle>Delete HocPhan</DialogTitle>
                  <DialogDescription>
                    Delete the current item.
                  </DialogDescription>
                </DialogHeader>
                <p>Are you sure you want to delete this HocPhan?</p>
                <DialogFooter>
                  <Button type="submit" onClick={() => handleDelete(item.id)}>
                    Delete
                  </Button>
                </DialogFooter>
              </DialogContent>
            </Dialog>
            <Dialog>
              <DialogTrigger asChild>
                <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                  Thêm PLO
                </DropdownMenuItem>
              </DialogTrigger>
              <DialogContent className="w-auto max-w-none">
                <DialogHeader>
                  <DialogTitle>
                    Thêm PLO có sẵn vào Học Phần
                  </DialogTitle>
                </DialogHeader>
                {console.log("item.id", item.id)}
                <AddPLOToHocPhanForm
                  ref={addPLOFormRef}
                  hocPhanId={item.id}
                />
                <DialogFooter>
                  <DialogClose asChild>
                    <Button
                      type="button"
                      variant="default"
                      onClick={() =>
                        addPLOFormRef.current.handleAddPLO()
                      }
                    >
                      Thêm
                    </Button>
                  </DialogClose>
                </DialogFooter>
              </DialogContent>
            </Dialog>
            <Dialog>
              <DialogTrigger asChild>
                <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                  Quản lý PLO
                </DropdownMenuItem>
              </DialogTrigger>
              <DialogContent className="w-auto max-w-none">
                <DialogHeader>
                  <DialogTitle>
                    Xem và xóa PLO khỏi Học Phần
                  </DialogTitle>
                </DialogHeader>
                <ManagePLOInHocPhanForm
                  ref={managePLOFormRef}
                  hocPhanId={item.id}
                />
                <DialogFooter>
                  <DialogClose asChild>
                    <Button
                      type="button"
                      variant="default"
                      onClick={() =>
                        managePLOFormRef.current.handleRemovePLO()
                      }
                    >
                      Xóa
                    </Button>
                  </DialogClose>
                </DialogFooter>
              </DialogContent>
            </Dialog>
          </DropdownMenuContent>
        </DropdownMenu>
      );
    },
  },
];

  return (
    <Layout>
      <div className="w-full">
        <ComboBox items={nganhItems} setItemId={setSelectedNganhId} initialItemId={selectedNganhId} />
        <DataTable
          entity="HocPhan"
          createColumns={createHocPhanColumns}
          data={Array.isArray(data) ? data : []}
          setData={setData}
          deleteItem={deleteHocPhan}
          columnToBeFiltered={"ten"}
          ItemForm={HocPhanForm}
        />
      </div>
    </Layout>
  );
}
