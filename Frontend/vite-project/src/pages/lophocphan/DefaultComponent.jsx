import DataTable from "@/components/DataTable";
import { ArrowUpDown, MoreHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  getAllLopHocPhans,
  deleteLopHocPhan,
} from "@/api/api-lophocphan";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger, DropdownMenuSeparator,
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
import { LopHocPhanForm } from "@/components/LopHocPhanForm";
import AddSinhVienToLopHocPhanForm from "@/components/AddSinhVienToLopHocPhanForm";
import ManageSinhVienInLopHocPhanForm from "@/components/ManageSinhVienInLopHocPhanForm";
import AddGiangVienToLopHocPhanForm from "@/components/AddGiangVienToLopHocPhanForm";
import ManageGiangVienInLopHocPhanForm from "@/components/ManageGiangVienInLopHocPhanForm";
import { useRef } from "react";

export default function LopHocPhanPage() {
  const addSinhVienFormRef = useRef(null);
  const manageSinhVienFormRef = useRef(null);
  const addGiangVienFormRef = useRef(null);
  const manageGiangVienFormRef = useRef(null);
  const createLopHocPhanColumns = (handleEdit, handleDelete, ) => [
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
      accessorKey: "hocPhanId",
      header: ({ column }) => {
        return (
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Học Phần Id
            <ArrowUpDown />
          </Button>
        );
      },
      cell: ({ row }) => <div className="px-4 py-2">{row.getValue("hocPhanId")}</div>,
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
                    Sửa Lớp Học Phần
                  </DropdownMenuItem>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                  <DialogHeader>
                    <DialogTitle>Edit Lop Hoc Phan</DialogTitle>
                    <DialogDescription>
                      Edit the current item.
                    </DialogDescription>
                  </DialogHeader>
                  <LopHocPhanForm lopHocPhanId={item.id} handleEdit={handleEdit} />
                </DialogContent>
              </Dialog>
              <Dialog>
                <DialogTrigger asChild>
                  <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                    Xóa Lớp Học Phần
                  </DropdownMenuItem>
                </DialogTrigger>
                <DropdownMenuSeparator />
                <DialogContent className="sm:max-w-[425px]">
                  <DialogHeader>
                    <DialogTitle>Delete Lop Hoc Phan</DialogTitle>
                    <DialogDescription>
                      Delete the current item.
                    </DialogDescription>
                  </DialogHeader>
                  <p>Are you sure you want to delete this Lop Hoc Phan?</p>
                  <DialogFooter>
                    <Button
                      type="submit"
                      onClick={() => handleDelete(item.id)}
                    >
                      Delete
                    </Button>
                  </DialogFooter>
                </DialogContent>
              </Dialog>
              <Dialog>
                <DialogTrigger asChild>
                  <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                    Thêm Sinh Viên
                  </DropdownMenuItem>
                </DialogTrigger>
                <DialogContent className="w-auto max-w-none">
                  <DialogHeader>
                    <DialogTitle>Thêm sinh viên có sẵn vào Lớp Học Phần</DialogTitle>
                  </DialogHeader>
                  <AddSinhVienToLopHocPhanForm
                    ref={addSinhVienFormRef}
                    lopHocPhanId={item.id}
                  />
                  <DialogFooter>
                    <DialogClose asChild>
                      <Button type="button" variant="default"
                        onClick={() => addSinhVienFormRef.current.handleAddSinhVien()}
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
                    Quản lý Sinh Viên
                  </DropdownMenuItem>
                </DialogTrigger>
                <DropdownMenuSeparator />
                <DialogContent className="w-auto max-w-none">
                  <DialogHeader>
                    <DialogTitle>Xem và xóa sinh viên khỏi lớp học phần</DialogTitle>
                  </DialogHeader>
                  <ManageSinhVienInLopHocPhanForm
                    ref={manageSinhVienFormRef}
                    lopHocPhanId={item.id}
                  />
                  <DialogFooter>
                    <DialogClose asChild>
                      <Button type="button" variant="default"
                        onClick={() => manageSinhVienFormRef.current.handleRemoveSinhVien()}
                      >
                        Xóa
                      </Button>
                    </DialogClose>
                  </DialogFooter>
                </DialogContent>
              </Dialog>
              <Dialog>
                <DialogTrigger asChild>
                  <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                    Thêm Giảng Viên
                  </DropdownMenuItem>
                </DialogTrigger>
                <DialogContent className="w-auto max-w-none">
                  <DialogHeader>
                    <DialogTitle>Thêm giảng viên có sẵn vào Lớp Học Phần</DialogTitle>
                  </DialogHeader>
                  <AddGiangVienToLopHocPhanForm
                    ref={addGiangVienFormRef}
                    lopHocPhanId={item.id}
                  />
                  <DialogFooter>
                    <DialogClose asChild>
                      <Button type="button" variant="default"
                        onClick={() => addGiangVienFormRef.current.handleAddGiangVien()}
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
                    Quản lý Giảng Viên
                  </DropdownMenuItem>
                </DialogTrigger>
                <DialogContent className="w-auto max-w-none">
                  <DialogHeader>
                    <DialogTitle>Xem và xóa giảng viên khỏi lớp học phần</DialogTitle>
                  </DialogHeader>
                  <ManageGiangVienInLopHocPhanForm
                    ref={manageGiangVienFormRef}
                    lopHocPhanId={item.id}
                  />
                  <DialogFooter>
                    <DialogClose asChild>
                      <Button type="button" variant="default"
                        onClick={() => manageGiangVienFormRef.current.handleRemoveGiangVien()}
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
    <div className="w-full">
      <DataTable
        entity="Lop Hoc Phan"
        createColumns={createLopHocPhanColumns}
        getAllItems={() => getAllLopHocPhans()}
        deleteItem={deleteLopHocPhan}
        columnToBeFiltered={"ten"}
        ItemForm={LopHocPhanForm}
      />
    </div>
  );
}
